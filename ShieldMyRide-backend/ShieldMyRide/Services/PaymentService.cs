using System;
using System.Linq;
using System.Threading.Tasks;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(int proposalId, decimal amount);
        Task<decimal> GetBalanceAmount(int proposalId);

        // New method to get detailed balance info
        Task<(decimal TotalPaid, decimal BalanceRemaining, ClaimStatus ClaimStatus)> GetBalanceDetailsAsync(int proposalId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IProposalRepository _proposalRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IClaimRepository _claimRepo;

        public PaymentService(IProposalRepository proposalRepo, IPaymentRepository paymentRepo, IClaimRepository claimRepo)
        {
            _proposalRepo = proposalRepo;
            _paymentRepo = paymentRepo;
            _claimRepo = claimRepo;
        }


        // Process payment (supports partial payments)
        public async Task<bool> ProcessPayment(int proposalId, decimal amount)
        {
            var proposal = await _proposalRepo.GetByIdAsync(proposalId);
            if (proposal == null)
                return false;

            var balance = await GetBalanceAmount(proposalId);

            if (amount <= 0 || amount > balance)
                return false; // Cannot pay negative or more than remaining balance

            // Record the payment
            var payment = new Payment
            {
                ProposalID = proposal.ProposalId,
                AmountPaid = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Completed",
                UserID = proposal.UserId
            };
            await _paymentRepo.AddAsync(payment);

            // Update proposal status if fully paid
            var newBalance = balance - amount;
            if (newBalance <= 0)
            {
                proposal.ProposalStatus = ProposalStatus.Active; // Fully paid
                await _proposalRepo.UpdateAsync(proposal);
            }

            return true;
        }

        // Get remaining balance based on proposal's claim settlement
        public async Task<decimal> GetBalanceAmount(int proposalId)
        {
            var claim = await _claimRepo.GetByProposalIdAsync(proposalId);
            if (claim == null)
                return 0;

            var payments = await _paymentRepo.GetByProposalIdAsync(proposalId);
            var totalPaid = payments?.Sum(p => p.AmountPaid) ?? 0;

            var balance = claim.SettlementAmount - totalPaid;
            return balance < 0 ? 0 : balance;
        }

        // Get balance details similar to GetBalance in controller
        public async Task<(decimal TotalPaid, decimal BalanceRemaining, ClaimStatus ClaimStatus)> GetBalanceDetailsAsync(int proposalId)
        {
            var claim = await _claimRepo.GetByProposalIdAsync(proposalId);
            if (claim == null)
                return (0, 0, ClaimStatus.Pending);

            var payments = await _paymentRepo.GetByProposalIdAsync(proposalId);
            var totalPaid = payments?.Sum(p => p.AmountPaid) ?? 0;

            var balance = claim.SettlementAmount - totalPaid;
            if (balance < 0) balance = 0;

            return (totalPaid, balance, claim.ClaimStatus);
        }
    }
}
