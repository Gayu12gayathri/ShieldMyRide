using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IOfficerAssignmentRepository
    {
        Task<OfficerAssignment> GetByIdAsync(int id);
        Task<IEnumerable<OfficerAssignment>> GetAllAsync();
        Task<IEnumerable<OfficerAssignment>> GetByOfficerIdAsync(int officerId);
        Task<IEnumerable<OfficerAssignment>> GetByClaimIdAsync(int claimId);
        Task<OfficerAssignment> GetByProposalIdAsync(int proposalId);
        Task AddAsync(OfficerAssignment assignment);
        Task UpdateAsync(OfficerAssignment assignment);
        Task DeleteAsync(int id);
        Task<bool> AssignmentExistsAsync(int id);
    }
}
