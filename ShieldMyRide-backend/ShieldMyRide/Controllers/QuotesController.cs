using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.DTOs.QuoteDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.Services;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IPremiumCalculator _premiumCalculator;

        public QuoteController(
            IQuoteRepository quoteRepository,
            IProposalRepository proposalRepository,
            IPremiumCalculator premiumCalculator)
        {
            _quoteRepository = quoteRepository;
            _proposalRepository = proposalRepository;
            _premiumCalculator = premiumCalculator;
        }

        // ✅ Officers/Admins generate a quote based on proposal
        [HttpPost("generate/{proposalId}")]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> GenerateQuote(int proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(proposalId);
                if (proposal == null)
                    return NotFound($"Proposal with ID {proposalId} not found.");

                string breakdown;
                decimal premium = _premiumCalculator.Calculate(
                    proposal.VehicleType ?? "car",
                    proposal.VehicleAge,
                    proposal.Policy?.CoverageAmount ?? proposal.Premium,
                    out breakdown,
                    proposal.ZeroDep,
                    proposal.RoadsideAssist,
                    proposal.NCBPercent
                );

                var quote = new Quote
                {
                    ProposalId = proposal.ProposalId,
                    PolicyId = proposal.PolicyId,
                    DateIssued = DateTime.Now,
                    GeneratedAt = DateTime.Now,
                    ValidTill = DateTime.Now.AddDays(30),
                    PremiumAmount = premium,
                    CoverageDetails = breakdown
                };

                await _quoteRepository.AddAsync(quote);

                // 🔹 FIX: Update proposal with new premium and status
                proposal.Premium = premium;
                proposal.ProposalStatus = ProposalStatus.QuoteGenerated;
                await _proposalRepository.UpdateAsync(proposal);

                return CreatedAtAction(nameof(GetQuote), new { id = quote.QuoteId }, new
                {
                    quote.QuoteId,
                    quote.ProposalId,
                    quote.PolicyId,
                    quote.PremiumAmount,
                    quote.ValidTill,
                    quote.CoverageDetails
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating quote: {ex.Message}");
            }
        }


        // ✅ Get all quotes (officer/admin only)
        [HttpGet]
        [Authorize(Roles = "Officer,Admin,User")]
        public async Task<IActionResult> GetAllQuotes()
        {
            try
            {
                var quotes = await _quoteRepository.GetAllAsync();
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching quotes: {ex.Message}");
            }
        }

        // ✅ Get single quote (user can only see their own, officers can see all)
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Officer,Admin")]
        public async Task<IActionResult> GetQuote(int id)
        {
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);
                if (quote == null) return NotFound();

                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching quote: {ex.Message}");
            }
        }

        // ✅ Update quote (Officer/Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> UpdateQuote(int id, [FromBody] QuoteUpdateDTO dto)
        {
            try
            {
                var existingQuote = await _quoteRepository.GetByIdAsync(id);
                if (existingQuote == null) return NotFound();

                existingQuote.PremiumAmount = dto.Premium;
                existingQuote.ValidTill = dto.ValidTill;

                await _quoteRepository.UpdateAsync(existingQuote);

                return Ok(existingQuote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating quote: {ex.Message}");
            }
        }

        //calculate quote in the frontend
        [AllowAnonymous]
        [HttpPost("calculate")]
        //[Authorize(Roles = "User,Officer,Admin")]
        public IActionResult CalculateQuote([FromBody] QuoteRequestDTO dto)
        {
            try
            {
                // Validate DTO
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Call the existing PremiumCalculator
                string breakdown;
                decimal premium = _premiumCalculator.Calculate(
                    dto.VehicleType,
                    dto.VehicleAge,
                    dto.CoverageAmount,
                    out breakdown,
                    dto.ZeroDep,
                    dto.RoadsideAssist,
                    (int)dto.NCBPercent
                );

                // Return as JSON
                return Ok(new
                {
                    PremiumAmount = premium,
                    CoverageDetails = breakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating quote: {ex.Message}");
            }
        }



        // ✅ Delete quote (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);
                if (quote == null) return NotFound();

                await _quoteRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting quote: {ex.Message}");
            }
        }
    }
}