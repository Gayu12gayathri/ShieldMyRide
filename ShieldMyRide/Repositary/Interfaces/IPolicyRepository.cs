using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IPolicyRepository
    {
        Task<Policy> GetByIdAsync(int id);
        Task<IEnumerable<Policy>> GetAllAsync();
        Task<IEnumerable<Policy>> GetByCoverageTypeAsync(string coverageType);
        Task AddAsync(Policy policy);
        Task UpdateAsync(Policy policy);
        Task DeleteAsync(int id);
        Task<bool> PolicyExistsAsync(int id);
    }
}
