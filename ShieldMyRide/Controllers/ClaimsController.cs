using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IProposalRepository _proposalRepository;

        public ClaimsController(IClaimRepository claimRepository, IProposalRepository proposalRepository)
        {
            _claimRepository = claimRepository;
            _proposalRepository = proposalRepository;
        }

        // GET ALL CLAIMS - Officer Only

    
        [HttpGet]
        public async Task<IActionResult> GetAllClaims()
        {
            try
            {
                var claims = await _claimRepository.GetAllAsync();
                return Ok(claims);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET CLAIM BY ID - User or Officer
        [Authorize(Roles = "User,Officer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaim(int id)
        {
            try
            {
                var claim = await _claimRepository.GetByIdAsync(id);
                if (claim == null) return NotFound();
                return Ok(claim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

   // CREATE CLAIM - User Only (Proposal must be approved)
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateClaim([FromBody] InsuranceClaim claim)
        {
            try
            {
                // Check if the proposal exists and is approved
                var proposal = await _proposalRepository.GetByIdAsync(claim.ProposalId);
                if (proposal == null) return NotFound("Proposal not found.");
                if (proposal.ProposalStatus != ProposalStatus.Approved)
                    return BadRequest("Claim cannot be submitted because the proposal is not approved.");

                claim.ClaimDate = DateTime.Now;

                // Validate Status: if invalid, default to Assigned
                if (!Enum.IsDefined(typeof(ClaimStatus), ClaimStatus.Pending))
                     claim.ClaimStatus = ClaimStatus.Pending;



                await _claimRepository.AddAsync(claim);
                return CreatedAtAction(nameof(GetClaim), new { id = claim.ClaimId }, claim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
       


        // UPDATE CLAIM - Officer Only
 
        [Authorize(Roles = "Officer,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaim(int id, [FromBody] InsuranceClaim claim)
        {
            try
            {
                var existingClaim = await _claimRepository.GetByIdAsync(id);
                if (existingClaim == null) return NotFound();

                // Officers can update description, amount, status, and settlement
                existingClaim.ClaimDescription = claim.ClaimDescription;
                existingClaim.ClaimAmount = claim.ClaimAmount;
                existingClaim.ClaimStatus = claim.ClaimStatus;
                existingClaim.SettlementAmount = claim.SettlementAmount;

                await _claimRepository.UpdateAsync(existingClaim);
                return Ok(existingClaim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        

        // DELETE CLAIM - Officer and user Only
        [Authorize(Roles = "Officer,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(int id)
        {
            try
            {
                var claim = await _claimRepository.GetByIdAsync(id);
                if (claim == null) return NotFound();

                await _claimRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
