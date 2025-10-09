using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShieldMyRide.Authentication;
using ShieldMyRide.Context;
using ShieldMyRide.Models;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly MyDBContext _context;
        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MyDBContext context, ILogger<AuthenticationController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            _logger.LogInformation("Register attempt for username: {Username}, email: {Email}", model.Username, model.Email);

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("Registration failed: User {Username} already exists.", model.Username);
                return BadRequest(new Response { Status = "Error", Message = "User already exists!" });
            }

            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                AadhaarHash = HashHelper.ComputeSha256Hash(model.AadhaarMasked),
                PanHash = HashHelper.ComputeSha256Hash(model.PanMasked)
            };

            var result = await userManager.CreateAsync(identityUser, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Registration failed for user {Username}. Errors: {Errors}", model.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User creation failed!" });
            }

            // Role assignment
            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
                _logger.LogInformation("Created new role: {Role}", model.Role);
            }

            await userManager.AddToRoleAsync(identityUser, model.Role);

            var customUser = new User
            {
                IdentityUserId = identityUser.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AadhaarNumber = model.AadhaarMasked,
                PanNumber = model.PanMasked,
                Role = model.Role,
                DateOfBirth = model.DateOfBirth,
                PasswordHash = identityUser.PasswordHash,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(customUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {Username} successfully registered with role {Role}.", model.Username, model.Role);

            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }





        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("Login attempt for username: {Username}", model.Username);

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Login failed for username: {Username}", model.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(UserRoles.User) && !userRoles.Contains(UserRoles.Admin) && !userRoles.Contains(UserRoles.Officer))
            {
                _logger.LogWarning("Login restricted for username: {Username}, roles: {Roles}", model.Username, string.Join(", ", userRoles));
                return Unauthorized(new { message = "Restricted access" });
            }

            var customUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (customUser == null)
            {
                _logger.LogError("Custom user record not found for username: {Username}, email: {Email}", model.Username, user.Email);
                return Unauthorized(new { message = "User not found in custom Users table" });
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", customUser.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, customUser.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            if (userRoles.Contains(UserRoles.Officer))
            {
                _logger.LogInformation("Officer login detected for username: {Username}", model.Username);
                authClaims.Add(new Claim("OfficerId", customUser.UserId.ToString()));
            }

            var authSigninKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddHours(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );

            _logger.LogInformation("Login successful for username: {Username}, roles: {Roles}", model.Username, string.Join(", ", userRoles));
            _logger.LogInformation("Returning userId {UserId} for {Username}", customUser?.UserId, model.Username);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                roles = userRoles,
                userId = customUser.UserId,
                username = $"{customUser.FirstName} {customUser.LastName}"

            });
        }


        private IActionResult Forbid(object value)
        {
            throw new NotImplementedException();
        }
    }
}
