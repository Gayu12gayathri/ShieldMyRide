using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.Services;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalsController : ControllerBase
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IOfficerAssignmentRepository _officerAssignmentRepository;
        private readonly IPremiumCalculator _premiumCalculator;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public ProposalsController(IProposalRepository proposalRepository,
                                   IOfficerAssignmentRepository officerAssignmentRepository,
                                   IPremiumCalculator premiumCalculator,
                                   IPaymentService paymentService,
                                   IMapper mapper)
        {
            _proposalRepository = proposalRepository;
            _officerAssignmentRepository = officerAssignmentRepository;
            _premiumCalculator = premiumCalculator;
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProposals()
        {
            try
            {
                var proposals = await _proposalRepository.GetAllAsync();
                if (proposals == null || !proposals.Any())
                    return NotFound("No proposals found.");

                return Ok(proposals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching proposals: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Officer")]
        public async Task<IActionResult> GetProposal(int id)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                return Ok(proposal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching proposal: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateProposal([FromBody] Proposal proposal)
        {
            try
            {
                if (proposal == null)
                    return BadRequest("Proposal data is required.");

                string breakdown;
                decimal premium = _premiumCalculator.Calculate(
                    proposal.VehicleType ?? "car",
                    proposal.VehicleAge,
                    proposal.Policy?.CoverageAmount ?? proposal.Premium,
                    out breakdown
                );

                proposal.Premium = premium;
                proposal.ProposalStatus = ProposalStatus.Submitted;
                proposal.CreatedAt = DateTime.Now;

                await _proposalRepository.AddAsync(proposal);

                return CreatedAtAction(nameof(GetProposal), new { id = proposal.ProposalId }, new
                {
                    proposal.ProposalId,
                    proposal.VehicleType,
                    proposal.VehicleAge,
                    proposal.Premium,
                    proposal.ProposalStatus,
                    breakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating proposal: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateProposal(int id, [FromBody] Proposal proposal)
        {
            try
            {
                var existingProposal = await _proposalRepository.GetByIdAsync(id);
                if (existingProposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                existingProposal.VehicleType = proposal.VehicleType;
                existingProposal.VehicleRegNo = proposal.VehicleRegNo;
                existingProposal.VehicleAge = proposal.VehicleAge;
                existingProposal.ProposalStatus = proposal.ProposalStatus;

                await _proposalRepository.UpdateAsync(existingProposal);
                return Ok(existingProposal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating proposal: {ex.Message}");
            }
        }

        [HttpPut("{id}/review")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> ReviewProposal(int id, [FromBody] OfficerReviewDTO dto)
        {
            try
            {
                var existingProposal = await _proposalRepository.GetByIdAsync(id);
                if (existingProposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                existingProposal.ProposalStatus = dto.ProposalStatus;

                await _proposalRepository.UpdateAsync(existingProposal);

                return Ok(new
                {
                    message = "Proposal reviewed successfully",
                    proposalId = id,
                    newStatus = existingProposal.ProposalStatus.ToString(),
                    officerRemarks = dto.Remarks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reviewing proposal: {ex.Message}");
            }
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] decimal amount)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                var success = await _paymentService.ProcessPayment(id, amount);
                if (success)
                    return Ok($"Payment successful! Proposal {id} is now active.");
                else
                    return BadRequest("Payment failed. Check proposal ID or amount.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing payment: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProposal(int id)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                await _proposalRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting proposal: {ex.Message}");
            }
        }
    }
}
