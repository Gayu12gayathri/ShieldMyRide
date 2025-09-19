using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.DTOs.ClaimDTO;
using ShieldMyRide.DTOs.PolicyDTO;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.DTOs.QuoteDTO;
using System.Security.Claims;

namespace ShieldMyRide.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CustomerController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public CustomerController(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // USER: Get own proposals
        [HttpGet("proposals")]
        public async Task<ActionResult<IEnumerable<ProposalDTO>>> GetUserProposals()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);

                var proposals = await _context.Proposals
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<ProposalDTO>>(proposals));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // USER: Get own claims
        [HttpGet("claims")]
        public async Task<ActionResult<IEnumerable<ClaimGetDTO>>> GetUserClaims()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);

                var claims = await _context.InsuranceClaims
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<ClaimGetDTO>>(claims));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // USER: Get own quotes
        [HttpGet("quotes")]
        public async Task<ActionResult<IEnumerable<QuoteGetDTO>>> GetUserQuotes()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);

                var quotes = await _context.Quotes
                     .Include(q => q.Proposal)
                     .Where(q => q.Proposal.UserId == userId) // ✅ direct check
                     .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<QuoteGetDTO>>(quotes));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // USER: Get own policies
        [HttpGet("policies")]
        public async Task<ActionResult<IEnumerable<PolicyGetDTO>>> GetUserPolicies()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

                int userId = int.Parse(userIdClaim);

                var policies = await _context.Policies
                    .Include(p => p.PolicyDocuments)
                    .Include(p => p.Proposals)
                    .Where(p => p.Proposals.Any(pr => pr.UserId == userId)) // ✅ correct
                    .ToListAsync();

                return Ok(_mapper.Map<IEnumerable<PolicyGetDTO>>(policies));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
