using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.Models;
using System.Security.Claims;

namespace ShieldMyRide.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficersController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public OfficersController(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //ADMIN: Get all officer assignments
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OfficerAdminDTO>>> GetAssignmentsForAdmin()
        {
            try
            {
                var assignments = await _context.OfficerAssignments
                    .Include(o => o.Officer)
                    .Include(o => o.Proposal).ThenInclude(p => p.User)
                    .Include(o => o.Claim)
                    .ToListAsync();

                var dto = _mapper.Map<List<OfficerAdminDTO>>(assignments);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // OFFICER: Get only their own assignments (from JWT claim)
        [HttpGet("officer")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult<IEnumerable<OfficerDTO>>> GetAssignmentsForOfficer()
        {
            try
            {
                // Extract OfficerId from JWT claim
                var officerIdClaim = User.FindFirst("OfficerId")?.Value;
                if (string.IsNullOrEmpty(officerIdClaim))
                    return Unauthorized(new { message = "OfficerId claim missing in token" });

                int officerId = int.Parse(officerIdClaim);

                var assignments = await _context.OfficerAssignments
                    .Include(o => o.Proposal).ThenInclude(p => p.User)
                    .Where(o => o.OfficerId == officerId)
                    .ToListAsync();

                var dto = _mapper.Map<List<OfficerDTO>>(assignments);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [NonAction]
        public async Task GetCustomer(string testUserId)
        {
            throw new NotImplementedException();
        }

        // OFFICER: Update assignment status & remarks when authorized
        [HttpPut("update-status/{id:int}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateAssignmentStatus(int id, [FromBody] OfficerUpdateDTO updateDto)
        {
            try
            {
                // Extract OfficerId from JWT claim
                var officerIdClaim = User.FindFirst("OfficerId")?.Value;
                if (officerIdClaim == null)
                    return Unauthorized(new { message = "OfficerId claim missing in token" });

                int officerId = int.Parse(officerIdClaim);

                var assignment = await _context.OfficerAssignments.FirstOrDefaultAsync(o => o.OfficerAssignmentId == id);

                if (assignment == null)
                    return NotFound(new { message = $"Assignment {id} not found" });

                // Security check: ensure officer is updating only their assignment
                if (assignment.OfficerId != officerId)
                    return Forbid();

                assignment.Status = updateDto.Status;
                assignment.Remarks = updateDto.Remarks;
                assignment.AssignedDate = DateTime.Now; // update timestamp

                await _context.SaveChangesAsync();

                return Ok(new { message = "Assignment updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public static implicit operator OfficersController(UserController v)
        {
            throw new NotImplementedException();
        }
    }
}
