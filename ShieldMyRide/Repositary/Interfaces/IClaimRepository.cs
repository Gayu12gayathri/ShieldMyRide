using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IClaimRepository
    {
        Task<InsuranceClaim> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceClaim>> GetAllAsync();
        Task<IEnumerable<InsuranceClaim>> GetByUserIdAsync(int userId);
        Task<IEnumerable<InsuranceClaim>> GetByStatusAsync(string status);
        Task AddAsync(InsuranceClaim claim);
        Task UpdateAsync(InsuranceClaim claim);
        Task DeleteAsync(int id);
        Task<bool> ClaimExistsAsync(int id);
    }
}
