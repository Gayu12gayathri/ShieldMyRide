using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly MyDBContext _context;

        public ClaimRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<InsuranceClaim> GetByIdAsync(int id)
        {
            return await _context.InsuranceClaims
                .Include(c => c.User)
                .Include(c => c.Proposal)
                .FirstOrDefaultAsync(c => c.ClaimId == id);
        }

        public async Task<IEnumerable<InsuranceClaim>> GetAllAsync()
        {
            return await _context.InsuranceClaims
                .Include(c => c.User)
                .Include(c => c.Proposal)
                .ToListAsync();
        }
        public async Task DeleteClaimAsync(int id)
        {
            var claim = await _context.InsuranceClaims
                .Include(c => c.OfficerAssignments)
                .Include(c => c.Proposal) // include optional if needed
                .FirstOrDefaultAsync(c => c.ClaimId == id);

            if (claim == null)
                throw new Exception($"Claim with ID {id} not found.");

            // Remove dependent entities
            if (claim.OfficerAssignments != null && claim.OfficerAssignments.Any())
                _context.OfficerAssignments.RemoveRange(claim.OfficerAssignments);

            // Remove the claim itself
            _context.InsuranceClaims.Remove(claim);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<InsuranceClaim>> GetByUserIdAsync(int userId)
        {
            return await _context.InsuranceClaims
                .Where(c => c.UserId == userId)
                .Include(c => c.Proposal)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsuranceClaim>> GetByStatusAsync(string status)
        {
            if (!Enum.TryParse<ClaimStatus>(status, true, out var parsedStatus))
            {
                throw new ArgumentException($"Invalid claim status: {status}");
            }

            return await _context.InsuranceClaims
                .Where(c => c.ClaimStatus == parsedStatus)
                .Include(c => c.User)
                .Include(c => c.Proposal)
                .ToListAsync();
        }


        public async Task AddAsync(InsuranceClaim claim)
        {
            await _context.InsuranceClaims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }

        public async Task<InsuranceClaim?> GetByProposalIdAsync(int proposalId)
        {
            return await _context.InsuranceClaims
                                 .Include(c => c.User)
                                 .Include(c => c.Proposal)
                                 .FirstOrDefaultAsync(c => c.ProposalId == proposalId);
        }



        public async Task UpdateAsync(InsuranceClaim claim)
        {
            _context.InsuranceClaims.Update(claim);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var claim = await GetByIdAsync(id);
            if (claim != null)
            {
                _context.InsuranceClaims.Remove(claim);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClaimExistsAsync(int id)
        {
            return await _context.InsuranceClaims.AnyAsync(c => c.ClaimId == id);
        }
    }
}
