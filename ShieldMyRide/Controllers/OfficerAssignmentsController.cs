using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
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
            var assignments = await _assignmentRepository.GetAllAsync();
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) return NotFound();
            return Ok(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> CreateAssignment([FromBody] OfficerAssignment assignment)
        {
            assignment.AssignedDate = DateTime.Now;
            assignment.Status = "Assigned";

            await _assignmentRepository.AddAsync(assignment);
            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.OfficerAssignmentId }, assignment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] OfficerAssignment assignment)
        {
            var existingAssignment = await _assignmentRepository.GetByIdAsync(id);
            if (existingAssignment == null) return NotFound();

            existingAssignment.Remarks = assignment.Remarks;
            existingAssignment.Status = assignment.Status;

            await _assignmentRepository.UpdateAsync(existingAssignment);
            return Ok(existingAssignment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) return NotFound();

            await _assignmentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
