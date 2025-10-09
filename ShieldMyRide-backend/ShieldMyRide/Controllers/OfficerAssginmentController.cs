using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.DTOs;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerAssignmentController : ControllerBase
    {
        private readonly IOfficersAssignmentRepository _repo;

        public OfficerAssignmentController(IOfficersAssignmentRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromBody] OfficerAssignmentCreateDTO dto)
        {
            var assignment = new OfficersAssignment
            {
                OfficerId = dto.OfficerId,
                ActionType = dto.ActionType,
                TargetId = dto.TargetId,
                Remarks = dto.Remarks,
                Status = dto.Status
            };

            var result = await _repo.CreateAssignmentAsync(assignment);
            return Ok(result);
        }

        [HttpGet("officer/{officerId}")]
        public async Task<IActionResult> GetAssignmentsByOfficer(string officerId)
        {
            var assignments = await _repo.GetAssignmentsByOfficerAsync(officerId);
            return Ok(assignments);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await _repo.GetAllAssignmentsAsync();
            return Ok(assignments);
        }
    }
}
