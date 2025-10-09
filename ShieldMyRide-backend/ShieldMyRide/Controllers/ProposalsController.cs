using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShieldMyRide.Context;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.DTOs.ProposalpolicyDocument;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Implementation;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.Services;

namespace ShieldMyRide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalsController : ControllerBase
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IPolicyDocumentRepository _policyDocumentRepository;
        private readonly IOfficerAssignmentRepository _officerAssignmentRepository;
        private readonly IPremiumCalculator _premiumCalculator;
        private readonly IPaymentService _paymentService;
        private readonly IWebHostEnvironment _environment;
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public ProposalsController(IProposalRepository proposalRepository,
            IPolicyDocumentRepository policyDocumentRepository,
                                   IOfficerAssignmentRepository officerAssignmentRepository,
                                   IPremiumCalculator premiumCalculator, IWebHostEnvironment environment,
                                   MyDBContext context,IPaymentService paymentService,
                                   IMapper mapper)
        {
            _proposalRepository = proposalRepository;
            _policyDocumentRepository = policyDocumentRepository;
            _officerAssignmentRepository = officerAssignmentRepository;
            _premiumCalculator = premiumCalculator;
            _environment = environment;
            _paymentService = paymentService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProposals()
        {
            try
            {
                var proposals = await _proposalRepository.GetAllAsync();
                if (proposals == null || !proposals.Any())
                    return NotFound("No proposals found.");

                return Ok(proposals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching proposals: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Officer")]
        public async Task<IActionResult> GetProposal(int id)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                return Ok(proposal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching proposal: {ex.Message}");
            }
        }
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserProposals()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null) return Unauthorized("User ID not found in token");
                int userId = int.Parse(userIdClaim.Value);


                //int userId = int.Parse(userIdClaim);
                var proposals = await _proposalRepository.GetByUserIdAsync(userId);

                if (proposals == null || !proposals.Any())
                    return NotFound("No proposals found for this user.");

                return Ok(proposals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching user proposals: {ex.Message}");
            }
        }
        [HttpGet("by-regno/{regNo}")]
        public async Task<IActionResult> GetProposalsByRegNo(string regNo)
        {
            if (string.IsNullOrEmpty(regNo))
                return BadRequest(new { message = "Registration number is required" });

            var proposals = await _proposalRepository.GetProposalsByRegNoAsync(regNo);

            if (proposals == null || !proposals.Any())
                return NotFound(new { message = "No proposals found for this registration number" });

            return Ok(proposals);
        }

        [HttpPost("renew/{proposalId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RenewProposal(int proposalId)
        {
            try
            {
                var existingProposal = await _proposalRepository.GetByIdAsync(proposalId);
                if (existingProposal == null)
                    return NotFound(new { message = "Proposal not found" });

                // Check if already expired or active renewal
                var today = DateTime.UtcNow;
                if (existingProposal.PolicyEndDate > today)
                    return BadRequest(new { message = "Policy is still active. You can renew only after expiry." });

                // Create new proposal based on existing
                var newProposal = new Proposal
                {
                    UserId = existingProposal.UserId,
                    PolicyId = existingProposal.PolicyId,
                    PolicyName = existingProposal.PolicyName,
                    VehicleRegNo = existingProposal.VehicleRegNo,
                    VehicleType = existingProposal.VehicleType,
                    VehicleAge = existingProposal.VehicleAge + 1,
                    ProposalStatus = ProposalStatus.Pending,
                    PolicyStartDate = today,
                    PolicyEndDate = today.AddYears(1),
                    Premium = CalculateRenewalPremium(existingProposal),
                    CreatedAt = DateTime.UtcNow
                };

                // Save renewal proposal
                await _proposalRepository.AddAsync(newProposal);

                // Role-based response message
                var role = User.IsInRole("Admin") ? "Admin" : "User";
                var responseMessage = role == "Admin"
                    ? "Renewal processed successfully by Admin."
                    : "Your renewal request has been successfully submitted.";

                return Ok(new
                {
                    message = responseMessage,
                    renewalProposal = newProposal
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }
            catch (Exception ex)
            {
                // Log exception details if logging is implemented
                return StatusCode(500, new { message = "An error occurred while processing the renewal.", error = ex.Message });
            }
        }

        [HttpPost("create-with-documents")]
        public async Task<IActionResult> CreateWithDocuments([FromForm] ProposalWithDocumentsFormModelDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1️⃣ Create Proposal entity
                var proposal = new Proposal
                {
                    UserId = GetCurrentUserId(),   // implement from token/session
                    PolicyId = dto.PolicyId,
                    PolicyName = dto.PolicyName,
                    VehicleType = dto.VehicleType,
                    VehicleRegNo = dto.VehicleRegNo,
                    VehicleAge = dto.VehicleAge,
                    Premium = dto.CoverageAmount,
                    ProposalStatus = ProposalStatus.Submitted,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Proposals.Add(proposal);
                await _context.SaveChangesAsync(); // ProposalId is generated

                // 2️⃣ Create folder to store uploaded files
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "uploads", "proposals", proposal.ProposalId.ToString()
                );
                Directory.CreateDirectory(uploadsFolder);

                // 3️⃣ Helper to save file and create PolicyDocument
                async Task<string?> SaveFileAsync(IFormFile? file, string docType)
                {
                    if (file == null) return null;

                    string fileName = $"{docType}_{DateTime.UtcNow.Ticks}{Path.GetExtension(file.FileName)}";
                    string fullPath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    string relativePath = Path.Combine("uploads", "proposals", proposal.ProposalId.ToString(), fileName);

                    // Add to PolicyDocuments table
                    var document = new PolicyDocument
                    {
                        ProposalId = proposal.ProposalId,
                        PolicyId = proposal.PolicyId,
                        DocumentType = docType,
                        DocumentPath = relativePath,
                        UploadedAt = DateTime.UtcNow,
                        IssuedDate = DateTime.UtcNow
                    };
                    _context.PolicyDocuments.Add(document);

                    return relativePath;
                }

                // 4️⃣ Save all uploaded files
                proposal.DrivingLicensePath = await SaveFileAsync(dto.DrivingLicense, "DrivingLicense");
                proposal.PreviousInsurancePath = await SaveFileAsync(dto.PreviousInsurance, "PreviousInsurance");
                proposal.IncomeProofPath = await SaveFileAsync(dto.IncomeProof, "IncomeProof");
                proposal.PassportPhotoPath = await SaveFileAsync(dto.PassportPhoto, "PassportPhoto");
                proposal.AddressProofPath = await SaveFileAsync(dto.AddressProof, "AddressProof");
                proposal.SignaturePath = await SaveFileAsync(dto.Signature, "Signature");

                // 5️⃣ Commit changes to DB
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "✅ Proposal and documents saved successfully",
                    proposalId = proposal.ProposalId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating proposal", details = ex.Message });
            }
        }

        // Example helper to get current userId from JWT
        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }


        //[HttpPost("create-with-documents")]
        //[Authorize] // optional - depends if auth token required
        //public async Task<IActionResult> CreateWithDocuments([FromForm] ProposalWithDocumentsFormModelDTO dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        // Get user ID from JWT
        //        var userIdClaim = User.FindFirst("UserId")?.Value;
        //        int userId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

        //        // ✅ Create Proposal object
        //        var proposal = new Proposal
        //        {
        //            UserId = userId,
        //            PolicyId = dto.PolicyId,
        //            PolicyName = dto.PolicyName,
        //            VehicleType = dto.VehicleType,
        //            VehicleRegNo = dto.VehicleRegNo,
        //            VehicleAge = dto.VehicleAge,
        //            Premium = dto.CoverageAmount,  // Using coverage amount as initial premium value
        //            ProposalStatus = ProposalStatus.Submitted,
        //            CreatedAt = DateTime.UtcNow,
        //            ZeroDep = false,
        //            RoadsideAssist = false,
        //            NCBPercent = 0
        //        };

        //        // ✅ Save proposal in DB
        //        await _proposalRepository.AddAsync(proposal);

        //        // ✅ Prepare upload directory
        //        var uploadPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Proposals", proposal.ProposalId.ToString());
        //        if (!Directory.Exists(uploadPath))
        //            Directory.CreateDirectory(uploadPath);

        //        // ✅ Helper to save each file
        //        async Task SaveFileAsync(IFormFile? file, string name)
        //        {
        //            if (file != null && file.Length > 0)
        //            {
        //                var filePath = Path.Combine(uploadPath, $"{name}{Path.GetExtension(file.FileName)}");
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await file.CopyToAsync(stream);
        //                }
        //            }
        //        }

        //        // ✅ Save all uploaded files
        //        await SaveFileAsync(dto.DrivingLicense, "DrivingLicense");
        //        await SaveFileAsync(dto.PreviousInsurance, "PreviousInsurance");
        //        await SaveFileAsync(dto.IncomeProof, "IncomeProof");
        //        await SaveFileAsync(dto.PassportPhoto, "PassportPhoto");
        //        await SaveFileAsync(dto.AddressProof, "AddressProof");
        //        await SaveFileAsync(dto.Signature, "Signature");

        //        return Ok(new
        //        {
        //            message = "Proposal created successfully.",
        //            proposalId = proposal.ProposalId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Error creating proposal", error = ex.Message });
        //    }
        //}


        private decimal CalculateRenewalPremium(Proposal existingProposal)
        {
            // Example logic: small discount for renewal
            decimal previousPremium = existingProposal.Premium;
            decimal discount = 0.05m; // 5% discount for renewals
            return Math.Round(previousPremium * (1 - discount), 2);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateProposal([FromBody] Proposal proposal)
        {
            try
            {
                if (proposal == null)
                    return BadRequest("Proposal data is required.");

                string breakdown;
                decimal premium = _premiumCalculator.Calculate(
                    proposal.VehicleType ?? "car",
                    proposal.VehicleAge,
                    proposal.Policy?.CoverageAmount ?? proposal.Premium,
                    out breakdown
                );

                proposal.Premium = premium;
                proposal.ProposalStatus = ProposalStatus.Submitted;
                proposal.CreatedAt = DateTime.Now;

                await _proposalRepository.AddAsync(proposal);

                return CreatedAtAction(nameof(GetProposal), new { id = proposal.ProposalId }, new
                {
                    proposal.ProposalId,
                    proposal.VehicleType,
                    proposal.VehicleAge,
                    proposal.Premium,
                    proposal.ProposalStatus,
                    breakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating proposal: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateProposal(int id, [FromBody] Proposal proposal)
        {
            try
            {
                var existingProposal = await _proposalRepository.GetByIdAsync(id);
                if (existingProposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                existingProposal.VehicleType = proposal.VehicleType;
                existingProposal.VehicleRegNo = proposal.VehicleRegNo;
                existingProposal.VehicleAge = proposal.VehicleAge;
                existingProposal.ProposalStatus = proposal.ProposalStatus;

                await _proposalRepository.UpdateAsync(existingProposal);
                return Ok(existingProposal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating proposal: {ex.Message}");
            }
        }

        [HttpPut("{id}/review")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> ReviewProposal(int id, [FromBody] OfficerReviewDTO dto)
        {
            try
            {
                var existingProposal = await _proposalRepository.GetByIdAsync(id);
                if (existingProposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                existingProposal.ProposalStatus = dto.ProposalStatus;
                existingProposal.OfficerRemarks = dto.Remarks;

                await _proposalRepository.UpdateAsync(existingProposal);

                return Ok(new
                {
                    message = "Proposal reviewed successfully",
                    proposalId = id,
                    newStatus = existingProposal.ProposalStatus.ToString(),
                    officerRemarks = dto.Remarks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reviewing proposal: {ex.Message}");
            }
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(int id, [FromBody] decimal amount)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                var success = await _paymentService.ProcessPayment(id, amount);
                if (success)
                    return Ok($"Payment successful! Proposal {id} is now active.");
                else
                    return BadRequest("Payment failed. Check proposal ID or amount.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing payment: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProposal(int id)
        {
            try
            {
                var proposal = await _proposalRepository.GetByIdAsync(id);
                if (proposal == null)
                    return NotFound($"Proposal with ID {id} not found.");

                await _proposalRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting proposal: {ex.Message}");
            }
        }

        [HttpGet("download-pdf/{proposalId}/{vehicleRegNo}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DownloadCombinedPolicyPDF(int proposalId, string vehicleRegNo)
        {
            var proposal = await _proposalRepository.GetByIdAsync(proposalId);
            if (proposal == null)
                return NotFound(new { message = "Proposal not found" });

            var docs = await _policyDocumentRepository.GetByProposalIdAsync(proposalId);
            if (docs == null || !docs.Any())
                return NotFound(new { message = "No policy documents found for this proposal" });

            using (var memoryStream = new MemoryStream())
            {
                // Create a new PDF document
                Document document = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph("Insurance Policy Document", titleFont));
                document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("dd-MM-yyyy")));
                document.Add(new Paragraph("\n"));

                // Proposal Info Section
                var subTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var textFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                document.Add(new Paragraph("Proposal Information", subTitle));
                document.Add(new Paragraph($"Proposal ID: {proposal.ProposalId}", textFont));
                document.Add(new Paragraph($"Vehicle Registration: {proposal.VehicleRegNo}", textFont));
                document.Add(new Paragraph($"Vehicle Type: {proposal.VehicleType}", textFont));
                document.Add(new Paragraph($"Policy Name: {proposal.PolicyName}", textFont));
                document.Add(new Paragraph($"Premium: ₹{proposal.Premium}", textFont));
                document.Add(new Paragraph($"Policy Period: {proposal.PolicyStartDate:dd-MM-yyyy} to {proposal.PolicyEndDate:dd-MM-yyyy}", textFont));
                document.Add(new Paragraph($"Status: {proposal.ProposalStatus}", textFont));
                document.Add(new Paragraph("\n\nAttached Policy Documents:\n", subTitle));

                // Merge each document (if PDF)
                foreach (var doc in docs)
                {
                    var filePath = Path.Combine("wwwroot/policies", doc.DocumentType);
                    if (System.IO.File.Exists(filePath) && Path.GetExtension(filePath).ToLower() == ".pdf")
                    {
                        var reader = new PdfReader(filePath);
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            document.NewPage();
                            var imported = writer.GetImportedPage(reader, i);
                            var content = writer.DirectContent;
                            content.AddTemplate(imported, 0, 0);
                        }
                        reader.Close();
                    }
                    else
                    {
                        document.Add(new Paragraph($"• {doc.DocumentType} (Non-PDF document attached separately)", textFont));
                    }
                }

                document.Close();

                // Return combined PDF
                var fileBytes = memoryStream.ToArray();
                var fileName = $"Policy_{vehicleRegNo}_{proposalId}.pdf";
                return File(fileBytes, "application/pdf", fileName);
            }
        }
    }
}
