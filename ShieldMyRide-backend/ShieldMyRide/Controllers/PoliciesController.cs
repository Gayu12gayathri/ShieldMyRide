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
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyRepository _policyRepository;

        public PoliciesController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Officer,User,Admin")]
        public async Task<IActionResult> GetAllPolicies()
        {
            var policies = await _policyRepository.GetAllAsync();
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicy(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);
            if (policy == null) return NotFound();
            return Ok(policy);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePolicy([FromBody] Policy policy)
        {
            policy.CreatedAt = DateTime.Now;
            await _policyRepository.AddAsync(policy);
            return CreatedAtAction(nameof(GetPolicy), new { id = policy.PolicyId }, policy);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] Policy policy)
        {
            var existingPolicy = await _policyRepository.GetByIdAsync(id);
            if (existingPolicy == null) return NotFound();

            existingPolicy.PolicyName = policy.PolicyName;
            existingPolicy.Description = policy.Description;
            existingPolicy.CoverageType = policy.CoverageType;
            existingPolicy.BasePremium = policy.BasePremium;
            existingPolicy.DurationMonths = policy.DurationMonths;
            existingPolicy.CoverageAmount = policy.CoverageAmount;
            existingPolicy.ModifiedAt = DateTime.Now;

            await _policyRepository.UpdateAsync(existingPolicy);
            return Ok(existingPolicy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);
            if (policy == null) return NotFound();

            await _policyRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
