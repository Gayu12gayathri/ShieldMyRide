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


        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MyDBContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;   

        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterModel model)
        //{
        //    var userExists = await userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response
        //        {
        //            Status = "Error",
        //            Message = "User already Exist!"
        //        });
        //    }
        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username,
        //        PhoneNumber = model.PhoneNumber,
        //        AadhaarHash = HashHelper.ComputeSha256Hash(model.Aadhaar),
        //        PanHash = HashHelper.ComputeSha256Hash(model.PanNumber)
        //    };

        //    var result = await userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            new Response
        //            {
        //                Status = "Error",
        //                Message = "User Creation Failed! Please Check the user details and try again later."
        //            });
        //    }
        //    if (model.Role == "User")
        //    {
        //        if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        //        }
        //        if (await roleManager.RoleExistsAsync(UserRoles.User))
        //            await userManager.AddToRoleAsync(user, UserRoles.User);

        //    }
        //    if (model.Role == "Admin")
        //    {
        //        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //        }
        //        if (await roleManager.RoleExistsAsync(UserRoles.Admin))

        //            await userManager.AddToRoleAsync(user, UserRoles.Admin);

        //    }
        //    return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        //}
        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await userManager.FindByNameAsync(model.Username);
        //    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await userManager.GetRolesAsync(user);


        //        var authClaims = new List<Claim>
        //        {  
        //            new Claim(ClaimTypes.Name,user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        //        };
        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var authSigninKey = new SymmetricSecurityKey(
        //            Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["Jwt:ValidIssuer"],
        //            audience: _configuration["Jwt:ValidAudience"],
        //            expires: DateTime.Now.AddHours(5),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigninKey,
        //            SecurityAlgorithms.HmacSha256)
        //            );

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }
        //    return Unauthorized();
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Check if Identity user already exists
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Status = "Error",
                    Message = "User already exists!"
                });
            }

            // Create Identity user (AspNetUsers)
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                AadhaarHash = HashHelper.ComputeSha256Hash(model.Aadhaar),
                PanHash = HashHelper.ComputeSha256Hash(model.PanNumber)
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check details and try again."
                    });
            }

            // Assign Role in Identity
            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            }
            await userManager.AddToRoleAsync(user, model.Role);

            // Create matching record in your custom Users table
            var customUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AadhaarNumber = model.Aadhaar,
                PanNumber = model.PanNumber,
                Role = model.Role,
                DateOfBirth = model.DateOfBirth,
                PasswordHash = user.PasswordHash, // still hashed
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(customUser);
            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var userRoles = await userManager.GetRolesAsync(user);

            // Restrict users with "User" role
            // add ! to access "user" to generate tokeen and remove it later "!"
            if (!userRoles.Contains(UserRoles.User) && !userRoles.Contains(UserRoles.Admin) && !userRoles.Contains(UserRoles.Officer))
            {
                return Unauthorized(new { message = "Restricted access" });
            }
            var customUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (customUser == null)
            {
                return Unauthorized(new { message = "User not found in custom Users table" });
            }

            // Only allowed roles can get JWT
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", customUser.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, customUser.UserId.ToString()), // ✅ FIX
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            if (userRoles.Contains(UserRoles.Officer))
            {
                // find the custom user row by email
                var custoMUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (custoMUser != null)
                {
                    authClaims.Add(new Claim("OfficerId", customUser.UserId.ToString()));
                }
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

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        private IActionResult Forbid(object value)
        {
            throw new NotImplementedException();
        }
    }
}
