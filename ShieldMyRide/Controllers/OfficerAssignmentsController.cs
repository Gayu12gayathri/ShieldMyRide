using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Implementation;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerAssignmentsController : ControllerBase
    {
        private readonly IOfficerAssignmentRepository _assignmentRepository;

        public OfficerAssignmentsController(IOfficerAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                var assignments = await _assignmentRepository.GetAllAsync();

                var result = assignments.Select(a => new OfficerAssignmentDTO
                {
                    OfficerAssignmentId = a.OfficerAssignmentId,
                    OfficerId = a.OfficerId,
                    OfficerName = a.Officer != null ? $"{a.Officer.FirstName} {a.Officer.LastName}" : "Unknown",
                    ProposalId = a.ProposalId,
                    ClaimId = a.ClaimId,
                    Remarks = a.Remarks,
                    AssignedDate = a.AssignedDate,
                    Status = a.Status
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Failed to fetch officer assignments.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.GetByIdAsync(id);
                if (assignment == null) return NotFound();

                var dto = new OfficerAssignmentDTO
                {
                    OfficerAssignmentId = assignment.OfficerAssignmentId,
                    OfficerId = assignment.OfficerId,
                    OfficerName = assignment.Officer != null ? $"{assignment.Officer.FirstName} {assignment.Officer.LastName}" : "Unknown",
                    ProposalId = assignment.ProposalId,
                    ClaimId = assignment.ClaimId,
                    Remarks = assignment.Remarks,
                    AssignedDate = assignment.AssignedDate,
                    Status = assignment.Status
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new { message = $"Error fetching assignment with ID {id}.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> CreateAssignment([FromBody] OfficerAssignment assignment)
        {
            try
            {
                assignment.AssignedDate = DateTime.Now;
                assignment.Status = "Assigned";

                await _assignmentRepository.AddAsync(assignment);
                return CreatedAtAction(nameof(GetAssignment), new { id = assignment.OfficerAssignmentId }, assignment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating assignment: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] OfficerAssignment assignment)
        {
            try
            {
                var existingAssignment = await _assignmentRepository.GetByIdAsync(id);
                if (existingAssignment == null) return NotFound();

                existingAssignment.Remarks = assignment.Remarks;
                existingAssignment.Status = assignment.Status;

                await _assignmentRepository.UpdateAsync(existingAssignment);
                return Ok(existingAssignment);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database error while updating assignment.", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Unexpected error while creating assignment.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.GetByIdAsync(id);
                if (assignment == null) return NotFound();

                await _assignmentRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database error while deleting assignment.", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting assignment: {ex.Message}");
            }
        }
    }
}
