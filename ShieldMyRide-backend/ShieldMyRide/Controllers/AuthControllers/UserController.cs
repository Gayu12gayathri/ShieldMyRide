using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Authentication;
using ShieldMyRide.DTOs.UsersDTO;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly MyDBContext _context;

        private readonly IProposalRepository _proposalRepo;
        private readonly IPaymentRepository _paymentRepo;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<UserController> logger,
            MyDBContext context,
            IProposalRepository proposalRepo,
            IPaymentRepository paymentRepo
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _context = context;

            _proposalRepo = proposalRepo;
            _paymentRepo = paymentRepo;
        }

        [HttpGet("customer/{id:int}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customUser = await _context.Users.FindAsync(id);
            if (customUser == null)
                return NotFound($"User with Id {id} not found.");

            var dto = _mapper.Map<CustomerDTO>(customUser);
            return Ok(dto);
        }


        [HttpGet("officer/{id:int}")]
        public async Task<ActionResult<OfficerDeatilDTO>> GetOfficer(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"Officer with Id {id} not found.");

            var dto = _mapper.Map<OfficerDeatilDTO>(user);
            return Ok(dto);
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<AdminDTo>> GetAdmin(int id)
        {
            return await GetUserByRole<AdminDTo>(id, "Admin");
        }

        // Helper: Get user by role
        private async Task<ActionResult<TDto>> GetUserByRole<TDto>(int customUserId, string roleName)
        {
            try
            {
                var customUser = await _context.Users.FindAsync(customUserId);
                if (customUser == null)
                    return NotFound($"{roleName} with Id {customUserId} not found.");

                var identityUser = await _userManager.FindByIdAsync(customUser.IdentityUserId);
                if (identityUser == null)
                    return NotFound($"Linked Identity user not found for {roleName} with Id {customUserId}.");

                var roles = await _userManager.GetRolesAsync(identityUser);
                if (!roles.Contains(roleName))
                    return BadRequest($"User with Id {customUserId} is not assigned role {roleName}.");

                var dto = _mapper.Map<TDto>(identityUser);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching {Role} with Id {UserId}", roleName, customUserId);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Balance Endpoint (by UserId only)

        [HttpGet("{userId:int}/balance")]
        public async Task<IActionResult> GetUserBalance(int userId)
        {
            try
            {
                //  Find user by UserId
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = $"User with Id {userId} not found." });

                //  Get proposals of this user
                var proposals = await _proposalRepo.GetByUserIdAsync(user.UserId);
                var result = new List<object>();

                foreach (var proposal in proposals)
                {
                    //  Get the claim for the proposal
                    var claim = await _context.InsuranceClaims
                        .Where(c => c.ProposalId == proposal.ProposalId)
                        .FirstOrDefaultAsync();

                    //  Use settlement amount if available, otherwise fallback to premium
                    decimal settlementAmount = claim?.SettlementAmount ?? proposal.Premium;

                    // Calculate total payments made for this proposal
                    var payments = await _paymentRepo.GetByProposalIdAsync(proposal.ProposalId);
                    decimal totalPaid = payments?.Sum(p => p.AmountPaid) ?? 0m;

                    
                    decimal balanceAmount = settlementAmount - totalPaid;
                    if (balanceAmount < 0) balanceAmount = 0;

                    string claimStatus = claim?.ClaimStatus.ToString() ?? "NotSubmitted";

                    result.Add(new
                    {
                        ProposalId = proposal.ProposalId,
                        UserId = user.UserId,
                        UserName = $"{user.FirstName} {user.LastName}",
                        ClaimedAmount = claim?.ClaimAmount ?? 0m,
                        SettlementAmount = settlementAmount,
                        TotalPaid = totalPaid,
                        BalanceAmount = balanceAmount,
                        ClaimStatus = claimStatus
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching balance for UserId {UserId}", userId);
                return StatusCode(500, new { message = "An error occurred while fetching balance.", details = ex.Message });
            }
        }

    }
}
