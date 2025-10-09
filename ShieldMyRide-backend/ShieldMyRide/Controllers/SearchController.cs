using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.DTOs.UsersDTO;
using ShieldMyRide.Models;

namespace ShieldMyRide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public SearchController(MyDBContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }


        // OFFICER SEARCH ENDPOINTS


        [HttpGet("officer/by-vehicle")]
        //[Authorize(Roles = "Officer,Admin,User")]
        public async Task<IActionResult> SearchByVehicle([FromQuery] string? regNo, [FromQuery] string? vehicleType)
        {
            IQueryable<Proposal> query = _context.Proposals.Include(p => p.User);

            if (!string.IsNullOrEmpty(regNo))
            {
                query = query.Where(p => p.VehicleRegNo == regNo);
            }

            if (!string.IsNullOrEmpty(vehicleType))
            {
                query = query.Where(p => p.VehicleType == vehicleType);
            }

            var results = await query.ToListAsync();

            if (!results.Any())
                return NotFound("No records found for the given search criteria.");

            return Ok(results);
        }


        [HttpGet("officer/by-email/{email}")]
        //[Authorize(Roles = "Officer,Admin,User")]
        public async Task<IActionResult> SearchByEmail(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound("No user found with given email.");

            var dto = _mapper.Map<UserSearchDTO>(user);
            return Ok(dto);
        }

        [HttpGet("officer/by-userid/{userId:int}")]
        //[Authorize(Roles = "Officer,Admin,User")]
        public async Task<IActionResult> SearchByUserId(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Proposals)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound("No user found with given UserId.");

            var dto = _mapper.Map<UserSearchDTO>(user);
            return Ok(dto);
        }

        [HttpGet("officer/by-proposal/{proposalId:int}")]
        //[Authorize(Roles = "Officer,Admin")]
        public async Task<IActionResult> SearchByProposal(int proposalId)
        {
            var proposal = await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.OfficerAssignments)
                .FirstOrDefaultAsync(p => p.ProposalId == proposalId);

            if (proposal == null)
                return NotFound("No proposal found.");

            return Ok(proposal);
        }


        // USER SEARCH ENDPOINTS


        [HttpGet("user/proposals/{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserProposals(int userId)
        {
            var proposals = await _context.Proposals
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (!proposals.Any())
                return NotFound("No proposals found for this user.");

            return Ok(proposals);
        }

        [HttpGet("user/claims/{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserClaims(int userId)
        {
            var claims = await _context.InsuranceClaims
                .Include(c => c.Proposal)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!claims.Any())
                return NotFound("No claims found for this user.");

            return Ok(claims);
        }
    }
}
