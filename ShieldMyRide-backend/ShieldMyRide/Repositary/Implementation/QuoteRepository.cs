using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly MyDBContext _context;

        public QuoteRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<Quote> GetByIdAsync(int id)
        {
            return await _context.Quotes
               .Include(q => q.Proposal)
                .FirstOrDefaultAsync(q => q.QuoteId == id);
        }

        public async Task<IEnumerable<Quote>> GetAllAsync()
        {
            return await _context.Quotes
                .Include(q => q.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetByPolicyIdAsync(int policyId)
        {
            return await _context.Quotes
                .Where(q => q.PolicyId == policyId)
                .Include(q => q.Policy)
                .ToListAsync();
        }
        public async Task<Quote> GetByProposalIdAsync(int proposalId)
        {
            return await _context.Quotes
                                 .Where(q => q.ProposalId == proposalId)
                                 .OrderByDescending(q => q.GeneratedAt) // latest quote
                                 .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Quote>> GetValidQuotesAsync()
        {
            return await _context.Quotes
                .Where(q => q.ValidTill >= DateTime.Now)
                .Include(q => q.Policy)
                .ToListAsync();
        }

        public async Task AddAsync(Quote quote)
        {
            await _context.Quotes.AddAsync(quote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quote = await GetByIdAsync(id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> QuoteExistsAsync(int id)
        {
            return await _context.Quotes.AnyAsync(q => q.QuoteId == id);
        }
    }
}
