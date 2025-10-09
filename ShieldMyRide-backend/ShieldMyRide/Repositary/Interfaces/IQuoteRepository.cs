using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote> GetByIdAsync(int id);
        Task<IEnumerable<Quote>> GetAllAsync();
        Task<IEnumerable<Quote>> GetByPolicyIdAsync(int policyId);
        Task<Quote> GetByProposalIdAsync(int proposalId);
        Task<IEnumerable<Quote>> GetValidQuotesAsync();
        Task AddAsync(Quote quote);
        Task UpdateAsync(Quote quote);
        Task DeleteAsync(int id);
        Task<bool> QuoteExistsAsync(int id);
    }
}
