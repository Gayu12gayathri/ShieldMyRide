using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.Authentication;
using ShieldMyRide.Repositary.Interfaces;
using System.Threading.Tasks;
using ShieldMyRide.Models;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found" });

            return Ok(user);
        }

        // GET: api/Users/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return NotFound(new { message = $"User with Email {email} not found" });

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Invalid user data" });

            if (await _userRepository.EmailExistsAsync(user.Email))
                return Conflict(new { message = "Email already exists" });

            await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (id != user.UserId)
                return BadRequest(new { message = "User ID mismatch" });

            if (!await _userRepository.UserExistsAsync(id))
                return NotFound(new { message = $"User with ID {id} not found" });

            await _userRepository.UpdateAsync(user);
            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userRepository.UserExistsAsync(id))
                return NotFound(new { message = $"User with ID {id} not found" });

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
