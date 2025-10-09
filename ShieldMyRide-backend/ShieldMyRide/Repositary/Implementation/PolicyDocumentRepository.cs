using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class PolicyDocumentRepository : IPolicyDocumentRepository
    {
        private readonly MyDBContext _context;

        public PolicyDocumentRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<PolicyDocument> GetByIdAsync(int id)
        {
            return await _context.PolicyDocuments
                .Include(pd => pd.Proposal)
                .Include(pd => pd.Policy)
                .FirstOrDefaultAsync(pd => pd.DocumentId == id);
        }

        public async Task<IEnumerable<PolicyDocument>> GetAllAsync()
        {
            return await _context.PolicyDocuments
                .Include(pd => pd.Proposal)
                .Include(pd => pd.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyDocument>> GetByProposalIdAsync(int proposalId)
        {
            return await _context.PolicyDocuments
                .Where(pd => pd.ProposalId == proposalId)
                .Include(pd => pd.Proposal)
                .Include(pd => pd.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyDocument>> GetByPolicyIdAsync(int policyId)
        {
            return await _context.PolicyDocuments
                .Where(pd => pd.PolicyId == policyId)
                .Include(pd => pd.Proposal)
                .Include(pd => pd.Policy)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyDocument>> GetByDocumentTypeAsync(string documentType)
        {
            return await _context.PolicyDocuments
                .Where(pd => pd.DocumentType == documentType)
                .Include(pd => pd.Proposal)
                .Include(pd => pd.Policy)
                .ToListAsync();
        }

        public async Task AddAsync(PolicyDocument policyDocument)
        {
            await _context.PolicyDocuments.AddAsync(policyDocument);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PolicyDocument policyDocument)
        {
            _context.PolicyDocuments.Update(policyDocument);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var policyDocument = await GetByIdAsync(id);
            if (policyDocument != null)
            {
                _context.PolicyDocuments.Remove(policyDocument);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PolicyDocumentExistsAsync(int id)
        {
            return await _context.PolicyDocuments.AnyAsync(pd => pd.DocumentId == id);
        }
    }
}
