using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public PaymentsController(IPaymentRepository paymentRepository, IProposalRepository proposalRepository)
        {
            _paymentRepository = paymentRepository;
            _proposalRepository = proposalRepository;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentRepository.GetAllAsync();
                if (payments == null || !payments.Any())
                    return NotFound(new { message = "No payments found." });

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Failed to fetch payments.", details = ex.Message });
            }
        }


        // GET: api/Payments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null)
                    return NotFound(new { message = $"No payment found with ID {id}." });

                var proposal = await _proposalRepository.GetByIdAsync(payment.ProposalID);
                decimal balanceAmount = 0;

                if (proposal != null)
                {
                    var allPayments = await _paymentRepository.GetByProposalIdAsync(payment.ProposalID);
                    decimal totalPaidUpToThisPayment = allPayments
                        .Where(p => p.PaymentDate <= payment.PaymentDate || p.PaymentId == payment.PaymentId)
                        .Sum(p => p.AmountPaid);

                    balanceAmount = proposal.Premium - totalPaidUpToThisPayment;
                    if (balanceAmount < 0) balanceAmount = 0;
                }

                return Ok(new
                {
                    payment.PaymentId,
                    payment.UserID,
                    payment.ProposalID,
                    payment.TransactionId,
                    payment.AmountPaid,
                    payment.PaymentDate,
                    payment.PaymentStatus,
                    BalanceAmount = balanceAmount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"Error fetching payment with ID {id}.", details = ex.Message });
            }
        }

        // GET: api/Payments/balance/{proposalId}
        [HttpGet("balance/{proposalId}")]
        public async Task<IActionResult> GetBalance(int proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(proposalId);
                if (proposal == null)
                    return NotFound(new { message = $"Proposal with ID {proposalId} not found." });

                var payments = await _paymentRepository.GetByProposalIdAsync(proposalId);
                decimal totalPaid = payments?.Sum(p => p.AmountPaid) ?? 0;
                decimal balance = proposal.Premium - totalPaid;
                if (balance < 0) balance = 0;

                return Ok(new
                {
                    ProposalId = proposalId,
                    TotalPremium = proposal.Premium,
                    TotalPaid = totalPaid,
                    Balance = balance
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Failed to fetch balance.", details = ex.Message });
            }
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            try
            {
                if (payment == null)
                    return BadRequest(new { message = "Payment data cannot be null." });

                if (await _paymentRepository.TransactionExistsAsync(payment.TransactionId))
                    return Conflict(new { message = $"Payment with Transaction ID {payment.TransactionId} already exists." });

                payment.PaymentDate = DateTime.Now;

                var proposal = await _proposalRepository.GetByIdAsync(payment.ProposalID);
                if (proposal == null)
                    return NotFound(new { message = $"Proposal with ID {payment.ProposalID} not found." });

                var payments = await _paymentRepository.GetByProposalIdAsync(payment.ProposalID);
                decimal totalPaid = payments?.Sum(p => p.AmountPaid) ?? 0;
                decimal remainingBalance = proposal.Premium - totalPaid;

                if (payment.AmountPaid > remainingBalance)
                    return BadRequest(new { message = $"Payment exceeds remaining balance of {remainingBalance}." });

                await _paymentRepository.AddAsync(payment);

                remainingBalance -= payment.AmountPaid;

                return Ok(new
                {
                    Payment = payment,
                    BalanceAmount = remainingBalance < 0 ? 0 : remainingBalance
                });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database error while creating payment.", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Unexpected error while creating payment.", details = ex.Message });
            }
        }

        // PUT: api/Payments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null)
                return NotFound(new { message = $"Payment with ID {id} not found." });

            var proposal = await _proposalRepository.GetByIdAsync(existingPayment.ProposalID);
            if (proposal == null)
                return NotFound(new { message = $"Proposal with ID {existingPayment.ProposalID} not found." });

            // Update only allowed fields
            existingPayment.AmountPaid = payment.AmountPaid;
            if (!string.IsNullOrEmpty(payment.PaymentStatus))
                existingPayment.PaymentStatus = payment.PaymentStatus;

            await _paymentRepository.UpdateAsync(existingPayment);

            // Recalculate total paid and remaining balance
            var payments = await _paymentRepository.GetByProposalIdAsync(existingPayment.ProposalID);
            decimal totalPaid = payments.Sum(p => p.AmountPaid);
            decimal balanceRemaining = proposal.Premium - totalPaid;

            return Ok(new
            {
                Payment = existingPayment,
                BalanceAmount = balanceRemaining < 0 ? 0 : balanceRemaining
            });
        }

        // DELETE: api/Payments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null)
                    return NotFound(new { message = $"Payment with ID {id} not found." });

                await _paymentRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database error while deleting payment.", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Unexpected error while deleting payment.", details = ex.Message });
            }
        }
    }
}
