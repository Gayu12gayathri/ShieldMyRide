using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Repositary.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MyDBContext _context;
        //private readonly IProposalRepository _proposalRepository;

        public PaymentRepository(MyDBContext context)
        {
            //_proposalRepository = proposalRepository;
            _context = context;
        }

        //geting the payment details using paymentid
        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Proposal)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }
        
        //get all the daatas of teh payment 
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Proposal)
                .ToListAsync();
        }
        //get by userid 

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserID == userId)
                .Include(p => p.Proposal)
                .ToListAsync();
        }

        //get by proposal id
        public async Task<IEnumerable<Payment>> GetByProposalIdAsync(int proposalId)
        {
            return await _context.Payments
                .Where(p => p.ProposalID == proposalId)
                .Include(p => p.User)
                .ToListAsync();
        }
        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<bool> TransactionExistsAsync(string transactionId)
        {
            return await _context.Payments.AnyAsync(p => p.TransactionId == transactionId);
        }


        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PaymentExistsAsync(int id)
        {
            return await _context.Payments.AnyAsync(p => p.PaymentId == id);
        }
    }
}
