using Microsoft.AspNetCore.Mvc;
using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IProposalRepository
    {
        Task<Proposal> GetByIdAsync(int id);
        Task<IEnumerable<Proposal>> GetAllAsync();
        Task<IEnumerable<Proposal>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Proposal>> GetByStatusAsync(ProposalStatus status);

        Task<IEnumerable<Proposal>> GetProposalsByRegNoAsync(string regNo);

        Task AddAsync(Proposal proposal);
        Task UpdateAsync(Proposal proposal);
        Task DeleteAsync(int id);
        Task<bool> ProposalExistsAsync(int id);
    }
}
