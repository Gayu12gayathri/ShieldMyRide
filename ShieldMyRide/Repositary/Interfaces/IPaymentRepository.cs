using ShieldMyRide.Models;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        //Task<bool> ProcessPaymentAsync(int proposalId, decimal amount);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetByProposalIdAsync(int proposalId);
        Task<Payment> GetByTransactionIdAsync(string transactionId);
        Task<bool> TransactionExistsAsync(string transactionId);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
        Task<bool> PaymentExistsAsync(int id);
    }
}
