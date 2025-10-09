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

        // GET all claims - Officer only
        [Authorize(Roles = "Officer,User,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllClaims()
        {
            var claims = await _claimRepository.GetAllAsync();
            return Ok(claims);
        }

        // GET claim by ID - User or Officer
        [Authorize(Roles = "User,Officer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaim(int id)
        {
            var claim = await _claimRepository.GetByIdAsync(id);
            if (claim == null) return NotFound();
            return Ok(claim);
        }

        // CREATE claim - User only (proposal must be approved)
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateClaim([FromBody] InsuranceClaim claim)
        {
            if (claim == null) return BadRequest("Claim data required.");

            var proposal = await _proposalRepository.GetByIdAsync(claim.ProposalId);
            if (proposal == null) return NotFound("Proposal not found.");
            if (proposal.ProposalStatus != ProposalStatus.Active)
                return BadRequest("Claim cannot be created because the proposal is not approved.");

            claim.ClaimDate = DateTime.Now;

            // Default status
            claim.ClaimStatus = ClaimStatus.Pending;

            // Set settlement amount automatically to proposal premium
            claim.ClaimAmount = 0;
            claim.SettlementAmount = proposal.Premium; ; // Initially unpaid

            await _claimRepository.AddAsync(claim);
            return CreatedAtAction(nameof(GetClaim), new { id = claim.ClaimId }, claim);
        }


        // UPDATE claim - Officer or User (description, amount, settlement, status)
        [Authorize(Roles = "Officer,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaim(int id, [FromBody] InsuranceClaim updatedClaim)
        {
            var existingClaim = await _claimRepository.GetByIdAsync(id);
            if (existingClaim == null) return NotFound("Claim not found.");

            // Update fields
            existingClaim.ClaimDescription = updatedClaim.ClaimDescription ?? existingClaim.ClaimDescription;
            existingClaim.SettlementAmount = updatedClaim.SettlementAmount > 0 ? updatedClaim.SettlementAmount : existingClaim.SettlementAmount;

            // Only allow manual override of status if needed
            if (Enum.IsDefined(typeof(ClaimStatus), updatedClaim.ClaimStatus))
                existingClaim.ClaimStatus = updatedClaim.ClaimStatus;

            await _claimRepository.UpdateAsync(existingClaim);
            return Ok(existingClaim);
        }

        // DELETE claim - Officer or User
        [Authorize(Roles = "Officer,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(int id)
        {
            var claim = await _claimRepository.GetByIdAsync(id);
            if (claim == null) return NotFound("Claim not found.");

            await _claimRepository.DeleteAsync(id);
            return NoContent();
        }

        // GET claim by proposalId - useful for payments controller
        [HttpGet("by-proposal/{proposalId}")]
        public async Task<IActionResult> GetClaimByProposal(int proposalId)
        {
            var claim = await _claimRepository.GetByProposalIdAsync(proposalId);
            if (claim == null) return NotFound("No claim found for this proposal.");
            return Ok(claim);
        }
    }
}