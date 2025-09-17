using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly MyDBContext _context;

        public PolicyRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<Policy> GetByIdAsync(int id)
        {
            return await _context.Policies.FindAsync(id);
        }

        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<IEnumerable<Policy>> GetByCoverageTypeAsync(string coverageType)
        {
            return await _context.Policies
                .Where(p => p.CoverageType == coverageType)
                .ToListAsync();
        }

        public async Task AddAsync(Policy policy)
        {
            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Policy policy)
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var policy = await GetByIdAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PolicyExistsAsync(int id)
        {
            return await _context.Policies.AnyAsync(p => p.PolicyId == id);
        }
    }
}
