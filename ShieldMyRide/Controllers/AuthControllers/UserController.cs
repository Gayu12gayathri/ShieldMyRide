using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.Authentication;
using ShieldMyRide.DTOs.UsersDTO;

namespace ShieldMyRide.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        //  Get Customer by Id
        [HttpGet("user/{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                return Ok(_mapper.Map<CustomerDTO>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //  Get Officer by Id
        [HttpGet("officer/{id}")]
        public async Task<ActionResult<OfficerDeatilDTO>> GetOfficer(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                return Ok(_mapper.Map<OfficerDeatilDTO>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //  Get Admin by Id
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<AdminDTo>> GetAdmin(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                return Ok(_mapper.Map<AdminDTo>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
