using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.DTOs.PaymentDTO;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IClaimRepository _claimRepository;
        private readonly IQuoteRepository _quoteRepository;

        public PaymentsController(
            IPaymentRepository paymentRepository,
            IProposalRepository proposalRepository,
            IClaimRepository claimRepository,
            IQuoteRepository quoteRepository)
        {
            _paymentRepository = paymentRepository;
            _proposalRepository = proposalRepository;
            _claimRepository = claimRepository;
            _quoteRepository = quoteRepository;
        }

        // 🔹 Helper: Updates Claim or Proposal after recalculation
        private async Task<(decimal Balance, string Status)> UpdateRelatedEntitiesAsync(
            int proposalId, bool forClaim, decimal totalPaid)
        {
            if (forClaim)
            {
                var claim = await _claimRepository.GetByProposalIdAsync(proposalId);
                if (claim == null) return (0, "ClaimNotFound");

                decimal balance = claim.SettlementAmount - totalPaid;
                if (balance < 0) balance = 0;

                // Keep track of total paid
                claim.ClaimAmount = totalPaid;

                if (balance == 0)
                    claim.ClaimStatus = ClaimStatus.Settled;
                else if (totalPaid > 0)
                    claim.ClaimStatus = ClaimStatus.PartiallyPaid;
                else
                    claim.ClaimStatus = ClaimStatus.Pending;

                await _claimRepository.UpdateAsync(claim);
                return (balance, claim.ClaimStatus.ToString());
            }
            else
            {
                var proposal = await _proposalRepository.GetByIdAsync(proposalId);
                var quote = await _quoteRepository.GetByProposalIdAsync(proposalId);
                if (proposal == null || quote == null) return (0, "ProposalOrQuoteNotFound");

                decimal balance = Math.Round(quote.PremiumAmount - totalPaid, 2);
                if (balance <= 0.01m) balance = 0;

                // Only change status if fully paid
                if (balance == 0)
                    proposal.ProposalStatus = ProposalStatus.Active;
                else if (proposal.ProposalStatus != ProposalStatus.Approved)
                    // keep Approved if already approved by officer
                    proposal.ProposalStatus = ProposalStatus.QuoteGenerated;

                await _proposalRepository.UpdateAsync(proposal);
                return (balance, proposal.ProposalStatus.ToString());
            }

        }

        // GET all payments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var result = new List<object>();

            foreach (var p in payments)
            {
                var totalPaid = (await _paymentRepository.GetByProposalIdAsync(p.ProposalID))
                                .Where(x => x.ForClaim == p.ForClaim)
                                .Sum(x => x.AmountPaid);

                var (balance, status) = await UpdateRelatedEntitiesAsync(p.ProposalID, p.ForClaim, totalPaid);

                result.Add(new
                {
                    p.PaymentId,
                    p.UserID,
                    p.ProposalID,
                    p.TransactionId,
                    p.AmountPaid,
                    p.PaymentDate,
                    p.PaymentStatus,
                    p.ForClaim,
                    Balance = balance,
                    Status = status
                });
            }

            return Ok(result);
        }

        // GET payment by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return NotFound();

            var totalPaid = (await _paymentRepository.GetByProposalIdAsync(payment.ProposalID))
                            .Where(x => x.ForClaim == payment.ForClaim)
                            .Sum(x => x.AmountPaid);

            var (balance, status) = await UpdateRelatedEntitiesAsync(payment.ProposalID, payment.ForClaim, totalPaid);

            return Ok(new
            {
                payment.PaymentId,
                payment.UserID,
                payment.ProposalID,
                payment.TransactionId,
                payment.AmountPaid,
                payment.PaymentDate,
                payment.PaymentStatus,
                payment.ForClaim,
                Balance = balance,
                Status = status
            });
        }

        // POST: create payment
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null)
                return BadRequest("Payment data required.");

            var payments = await _paymentRepository.GetByProposalIdAsync(payment.ProposalID);
            decimal totalPaidBefore = payments.Where(p => p.ForClaim == payment.ForClaim).Sum(p => p.AmountPaid);

            // Validation
            decimal baseAmount;
            if (payment.ForClaim)
            {
                var claim = await _claimRepository.GetByProposalIdAsync(payment.ProposalID);
                if (claim == null) return NotFound("Claim not found.");
                baseAmount = claim.SettlementAmount;
            }
            else
            {
                var quote = await _quoteRepository.GetByProposalIdAsync(payment.ProposalID);
                if (quote == null) return NotFound("Quote not found.");
                baseAmount = quote.PremiumAmount;
            }

            decimal remaining = baseAmount - totalPaidBefore;
            if (payment.AmountPaid > remaining)
                return BadRequest($"Payment exceeds remaining balance of {remaining}");

            // Save payment
            await _paymentRepository.AddAsync(payment);

            decimal totalPaidAfter = totalPaidBefore + payment.AmountPaid;
            var (balance, status) = await UpdateRelatedEntitiesAsync(payment.ProposalID, payment.ForClaim, totalPaidAfter);

            return Ok(new
            {
                Payment = payment,
                BalanceAmount = balance,
                Status = status
            });
        }

        // PUT: update payment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDTO dto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null)
                return NotFound(new { message = $"Payment with ID {id} not found." });

            var payments = await _paymentRepository.GetByProposalIdAsync(existingPayment.ProposalID);
            decimal totalPaidBeforeUpdate = payments
                .Where(p => p.PaymentId != id && p.ForClaim == existingPayment.ForClaim)
                .Sum(p => p.AmountPaid);

            //decimal baseAmount = existingPayment.ForClaim
            //    ? (await _claimRepository.GetByProposalIdAsync(existingPayment.ProposalID)).SettlementAmount
            //    : (await _quoteRepository.GetByProposalIdAsync(existingPayment.ProposalID)).PremiumAmount;
            decimal baseAmount;
            if (existingPayment.ForClaim)
            {
                var claim = await _claimRepository.GetByProposalIdAsync(existingPayment.ProposalID);
                if (claim == null) return NotFound("Claim not found");
                baseAmount = claim.SettlementAmount;
            }
            else
            {
                var quote = await _quoteRepository.GetByProposalIdAsync(existingPayment.ProposalID);
                if (quote == null) return NotFound("Quote not found");
                baseAmount = quote.PremiumAmount;
            }

            decimal remainingBeforeUpdate = baseAmount - totalPaidBeforeUpdate;
            if (dto.AmountPaid.HasValue && dto.AmountPaid.Value > remainingBeforeUpdate)
                return BadRequest($"Payment exceeds remaining balance. You can only pay up to {remainingBeforeUpdate}");

            // Update payment
            if (dto.AmountPaid.HasValue)
                existingPayment.AmountPaid += dto.AmountPaid.Value;

            if (!string.IsNullOrEmpty(dto.PaymentStatus))
                existingPayment.PaymentStatus = dto.PaymentStatus;

            if (!string.IsNullOrEmpty(dto.TransactionId))
                existingPayment.TransactionId = dto.TransactionId;

            await _paymentRepository.UpdateAsync(existingPayment);

            decimal totalPaidAfterUpdate = totalPaidBeforeUpdate + existingPayment.AmountPaid;
            var (balance, status) = await UpdateRelatedEntitiesAsync(existingPayment.ProposalID, existingPayment.ForClaim, totalPaidAfterUpdate);

            return Ok(new
            {
                Payment = existingPayment,
                BalanceAmount = balance,
                Status = status
            });
        }

        // DELETE: delete payment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null)
                return NotFound(new { message = $"Payment with ID {id} not found." });

            await _paymentRepository.DeleteAsync(id);

            var payments = await _paymentRepository.GetByProposalIdAsync(existingPayment.ProposalID);
            decimal totalPaidAfterDelete = payments.Where(p => p.ForClaim == existingPayment.ForClaim).Sum(p => p.AmountPaid);

            var (balance, status) = await UpdateRelatedEntitiesAsync(existingPayment.ProposalID, existingPayment.ForClaim, totalPaidAfterDelete);

            return Ok(new
            {
                Message = "Payment deleted successfully.",
                BalanceAmount = balance,
                Status = status
            });
        }
    }
}