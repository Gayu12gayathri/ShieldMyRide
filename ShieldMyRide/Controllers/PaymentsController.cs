using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(id);
                if (payment == null)
                    return NotFound(new { message = $"No payment found with ID {id}." });
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"Error fetching payment with ID {id}.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            try
            {
                if (payment == null)
                    return BadRequest(new { message = "Payment data cannot be null." });

                // Business rule: prevent duplicate payments with same transaction reference
                if (await _paymentRepository.TransactionExistsAsync(payment.TransactionId))
                    return Conflict(new { message = $"Payment with Transaction ID {payment.TransactionId} already exists." });

                payment.PaymentDate = DateTime.Now;

                await _paymentRepository.AddAsync(payment);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            try
            {
                var existingPayment = await _paymentRepository.GetByIdAsync(id);
                if (existingPayment == null)
                    return NotFound(new { message = $"Payment with ID {id} not found." });

                existingPayment.AmountPaid = payment.AmountPaid;
                existingPayment.PaymentStatus = payment.PaymentStatus;

                await _paymentRepository.UpdateAsync(existingPayment);
                return Ok(existingPayment);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database error while updating payment.", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Unexpected error while updating payment.", details = ex.Message });
            }
        }

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
