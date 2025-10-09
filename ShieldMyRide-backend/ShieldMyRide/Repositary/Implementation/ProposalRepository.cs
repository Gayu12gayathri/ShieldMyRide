using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly MyDBContext _context;
        //private readonly IProposalRepository _proposalRepository;
        public ProposalRepository(MyDBContext context)

        {
             //_proposalRepository = proposalRepository;
            _context = context;
        }

        public async Task<Proposal> GetByIdAsync(int id)
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.Policy)
                .Include(p => p.Quotes)
                .FirstOrDefaultAsync(p => p.ProposalId == id);
        }

        public async Task<IEnumerable<Proposal>> GetProposalsByRegNoAsync(string regNo)
        {
            return await _context.Proposals
                .Include(p => p.Policy)
                .Include(p => p.Quotes)
                .Include(p => p.User)
                .Where(p => p.VehicleRegNo == regNo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync()
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proposal>> GetByUserIdAsync(int userId)
        {
            return await _context.Proposals
                 .Include(p => p.Quotes)
                .Where(p => p.UserId == userId)
                .Include(p => p.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proposal>> GetByStatusAsync(ProposalStatus status)
        {
            return await _context.Proposals
                .Where(p => p.ProposalStatus == status)
                .Include(p => p.User)
                .Include(p => p.Policy)
                .ToListAsync();
        }


        public async Task AddAsync(Proposal proposal)
        {
            await _context.Proposals.AddAsync(proposal);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Proposal proposal)
        {
            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var proposal = await GetByIdAsync(id);
            if (proposal != null)
            {
                _context.Proposals.Remove(proposal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProposalExistsAsync(int id)
        {
            return await _context.Proposals.AnyAsync(p => p.ProposalId == id);
        }
    }
}
