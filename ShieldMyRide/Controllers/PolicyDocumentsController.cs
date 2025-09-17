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
    public class PolicyDocumentsController : ControllerBase
    {
        private readonly IPolicyDocumentRepository _policyDocumentRepository;

        public PolicyDocumentsController(IPolicyDocumentRepository policyDocumentRepository)
        {
            _policyDocumentRepository = policyDocumentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPolicyDocuments()
        {
            var documents = await _policyDocumentRepository.GetAllAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetPolicyDocument(int id)
        {
            var document = await _policyDocumentRepository.GetByIdAsync(id);
            if (document == null) return NotFound();
            return Ok(document);
        }

        [HttpGet("proposal/{proposalId}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> GetDocumentsByProposal(int proposalId)
        {
            var documents = await _policyDocumentRepository.GetByProposalIdAsync(proposalId);
            return Ok(documents);
        }

        [HttpGet("policy/{policyId}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> GetDocumentsByPolicy(int policyId)
        {
            var documents = await _policyDocumentRepository.GetByPolicyIdAsync(policyId);
            return Ok(documents);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreatePolicyDocument([FromBody] PolicyDocument document)
        {
            await _policyDocumentRepository.AddAsync(document);
            return CreatedAtAction(nameof(GetPolicyDocument), new { id = document.DocumentId }, document);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePolicyDocument(int id, [FromBody] PolicyDocument document)
        {
            var existingDocument = await _policyDocumentRepository.GetByIdAsync(id);
            if (existingDocument == null) return NotFound();

            existingDocument.DocumentPath = document.DocumentPath;
            existingDocument.DocumentType = document.DocumentType;
            existingDocument.IssuedDate = document.IssuedDate;

            await _policyDocumentRepository.UpdateAsync(existingDocument);
            return Ok(existingDocument);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicyDocument(int id)
        {
            var document = await _policyDocumentRepository.GetByIdAsync(id);
            if (document == null) return NotFound();

            await _policyDocumentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
