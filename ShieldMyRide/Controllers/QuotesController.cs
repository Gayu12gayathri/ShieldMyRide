using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuotesController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotes()
        {
            try
            {
                var quotes = await _quoteRepository.GetAllAsync();
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuote(int id)
        {
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);
                if (quote == null) return NotFound();
                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateQuote([FromBody] Quote quote)
        {
            try
            {
                quote.DateIssued = DateTime.Now;
                quote.GeneratedAt = DateTime.Now;
                quote.ValidTill = DateTime.Now.AddDays(30);

                await _quoteRepository.AddAsync(quote);
                return CreatedAtAction(nameof(GetQuote), new { id = quote.QuoteId }, quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateQuote(int id, [FromBody] Quote quote)
        {
            try
            {
                var existingQuote = await _quoteRepository.GetByIdAsync(id);
                if (existingQuote == null) return NotFound();

                existingQuote.PremiumAmount = quote.PremiumAmount;
                existingQuote.CoverageDetails = quote.CoverageDetails;
                existingQuote.ValidTill = quote.ValidTill;

                await _quoteRepository.UpdateAsync(existingQuote);
                return Ok(existingQuote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);
                if (quote == null) return NotFound();

                await _quoteRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
