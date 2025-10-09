using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class OfficerAssignmentsRepository : IOfficerAssignmentRepository
    {
        private readonly MyDBContext _context;

        public OfficerAssignmentsRepository(MyDBContext context)
        {
            _context = context;
        }
        public async Task<OfficerAssignment> GetByProposalIdAsync(int proposalId)
        {
            return await _context.OfficerAssignments
                .FirstOrDefaultAsync(oa => oa.ProposalId == proposalId);
        }
        public async Task<OfficerAssignment> GetByIdAsync(int id)
        {
            return await _context.OfficerAssignments
                .Include(o => o.Officer)
                .Include(o => o.Proposal)
                .Include(o => o.Claim)
                .FirstOrDefaultAsync(o => o.OfficerAssignmentId == id);
        }

        public async Task<IEnumerable<OfficerAssignment>> GetAllAsync()
        {
            return await _context.OfficerAssignments
                .Include(o => o.Officer)
                .Include(o => o.Proposal)
                .Include(o => o.Claim)
                .ToListAsync();
        }

        public async Task<IEnumerable<OfficerAssignment>> GetByOfficerIdAsync(int officerId)
        {
            return await _context.OfficerAssignments
                .Where(o => o.OfficerId == officerId)
                .Include(o => o.Proposal)
                .Include(o => o.Claim)
                .ToListAsync();
        }

        public async Task<IEnumerable<OfficerAssignment>> GetByClaimIdAsync(int claimId)
        {
            return await _context.OfficerAssignments
                .Where(o => o.ClaimId == claimId)
                .Include(o => o.Officer)
                .Include(o => o.Proposal)
                .ToListAsync();
        }

        public async Task AddAsync(OfficerAssignment assignment)
        {
            await _context.OfficerAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OfficerAssignment assignment)
        {
            _context.OfficerAssignments.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var assignment = await GetByIdAsync(id);
            if (assignment != null)
            {
                _context.OfficerAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AssignmentExistsAsync(int id)
        {
            return await _context.OfficerAssignments.AnyAsync(o => o.OfficerAssignmentId == id);
        }
    }
}
