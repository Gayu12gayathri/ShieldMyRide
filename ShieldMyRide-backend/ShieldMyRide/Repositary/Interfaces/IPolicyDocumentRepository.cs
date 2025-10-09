using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IPolicyDocumentRepository
    {
        Task<PolicyDocument> GetByIdAsync(int id);
        Task<IEnumerable<PolicyDocument>> GetAllAsync();
        Task<IEnumerable<PolicyDocument>> GetByProposalIdAsync(int proposalId);
        Task<IEnumerable<PolicyDocument>> GetByPolicyIdAsync(int policyId);
        Task<IEnumerable<PolicyDocument>> GetByDocumentTypeAsync(string documentType);
        Task AddAsync(PolicyDocument policyDocument);
        Task UpdateAsync(PolicyDocument policyDocument);
        Task DeleteAsync(int id);
        Task<bool> PolicyDocumentExistsAsync(int id);
    }
}
