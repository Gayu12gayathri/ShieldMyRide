using System;
using System.Threading.Tasks;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(int proposalId, decimal amount);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IProposalRepository _proposalRepo;
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IProposalRepository proposalRepo, IPaymentRepository paymentRepo)
        {
            _proposalRepo = proposalRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task<bool> ProcessPayment(int proposalId, decimal amount)
        {
            var proposal = await _proposalRepo.GetByIdAsync(proposalId);
            if (proposal == null || proposal.Premium != amount)
                return false;

            // Update proposal status
            proposal.ProposalStatus = ProposalStatus.Active;
            await _proposalRepo.UpdateAsync(proposal);

            // Record payment
            var payment = new Payment
            {
                ProposalID = proposal.ProposalId,
                AmountPaid = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Completed",
                UserID = proposal.UserId
            };
            await _paymentRepo.AddAsync(payment);

            // TODO: trigger document generation + email
            return true;
        }
    }
}
