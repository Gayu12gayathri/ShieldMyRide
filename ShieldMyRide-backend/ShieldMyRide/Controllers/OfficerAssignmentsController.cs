using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerAssignmentsController : ControllerBase
    {
        private readonly IOfficerAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;

        public OfficerAssignmentsController(
            IOfficerAssignmentRepository assignmentRepository,
            IUserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
        } 
        // GET all assignments (Admin & Officer)
        [HttpGet]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                var assignments = await _assignmentRepository.GetAllAsync();

                var dtoList = assignments.Select(a => new OfficerAssignmentDTO
                {
                    OfficerAssignmentId = a.OfficerAssignmentId,
                    OfficerId = a.OfficerId,
                    OfficerName = a.Officer != null ? $"{a.Officer.FirstName} {a.Officer.LastName}" : "Unknown",
                    ProposalId = a.ProposalId,
                    ClaimId = a.ClaimId,
                    Remarks = a.Remarks,
                    AssignedDate = a.AssignedDate,
                    Status = a.Status // use entity value to avoid cast issues
                }).ToList();

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Failed to fetch officer assignments.", details = ex.Message });
            }
        }

        // GET assignment by ID (Officer)
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

        // CREATE assignment (Officer or Admin can assign officers)
        [HttpPost]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> CreateAssignment([FromBody] OfficerAssignment assignment)
        {
            try
            {

                // Fetch officer details from the user repository
                var officer = await _userRepository.GetByIdAsync(assignment.OfficerId);

                // Check if the officer exists and has the "Officer" role
                if (officer == null || officer.Role != "Officer")
                {
                    return BadRequest(new { message = "Only officers can be assigned." });
                }

                // Proceed with creating the assignment
                assignment.AssignedDate = DateTime.Now;
                // Validate Status: if invalid, default to Assigned
                if (!Enum.IsDefined(typeof(OfficerStatus), assignment.Status))
                    assignment.Status = OfficerStatus.Assigned;

                // Add the assignment to the repository
                await _assignmentRepository.AddAsync(assignment);

                return CreatedAtAction(nameof(GetAssignment), new { id = assignment.OfficerAssignmentId }, assignment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error creating assignment.", details = ex.Message });
            }
        }


        // UPDATE assignment (Officer)
        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] OfficerAssignment assignment)
        {
            try
            {
                var existingAssignment = await _assignmentRepository.GetByIdAsync(id);
                if (existingAssignment == null) return NotFound();

                existingAssignment.Remarks = assignment.Remarks;

                // Safely assign enum value
                if (Enum.IsDefined(typeof(OfficerStatus), assignment.Status))
                    existingAssignment.Status = assignment.Status;

                await _assignmentRepository.UpdateAsync(existingAssignment);
                return Ok(existingAssignment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"Error updating assignment with ID {id}.", details = ex.Message });
            }
        }

        // DELETE assignment (Officer)
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"Error deleting assignment with ID {id}.", details = ex.Message });
            }
        }
    }
}
