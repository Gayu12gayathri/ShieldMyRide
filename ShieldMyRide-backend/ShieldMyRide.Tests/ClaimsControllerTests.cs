using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Controllers;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;

namespace ShieldMyRide.Tests.Controllers
{
    [TestFixture]
    public class ClaimsControllerTests
    {
        private Mock<IClaimRepository> _mockClaimRepo;
        private Mock<IProposalRepository> _mockProposalRepo;
        private ClaimsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockClaimRepo = new Mock<IClaimRepository>();
            _mockProposalRepo = new Mock<IProposalRepository>();
            _controller = new ClaimsController(_mockClaimRepo.Object, _mockProposalRepo.Object);
        }

        // -------------------- GET ALL --------------------
        [Test]
        public async Task GetAllClaims_ReturnsOk_WithClaims()
        {
            var claims = new List<InsuranceClaim>
            {
                new InsuranceClaim { ClaimId = 1, ClaimDescription = "Accident" },
                new InsuranceClaim { ClaimId = 2, ClaimDescription = "Theft" }
            };
            _mockClaimRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(claims);

            var result = await _controller.GetAllClaims();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            var list = ok.Value as List<InsuranceClaim>;
            Assert.That(list.Count, Is.EqualTo(2));
        }

        // -------------------- GET BY ID --------------------
        [Test]
        public async Task GetClaim_ReturnsOk_WhenExists()
        {
            var claim = new InsuranceClaim { ClaimId = 1, ClaimDescription = "Accident" };
            _mockClaimRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(claim);

            var result = await _controller.GetClaim(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That((ok.Value as InsuranceClaim).ClaimDescription, Is.EqualTo("Accident"));
        }

        [Test]
        public async Task GetClaim_ReturnsNotFound_WhenNotExists()
        {
            _mockClaimRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((InsuranceClaim)null);

            var result = await _controller.GetClaim(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        // -------------------- CREATE --------------------
        [Test]
        public async Task CreateClaim_ReturnsCreatedAtAction_WhenValid()
        {
            var proposal = new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.Active, Premium = 5000 };
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposal);

            var claim = new InsuranceClaim { ProposalId = 1, ClaimDescription = "Accident" };
            _mockClaimRepo.Setup(r => r.AddAsync(claim)).Returns(Task.CompletedTask);

            var result = await _controller.CreateClaim(claim);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var created = result as CreatedAtActionResult;
            var createdClaim = created.Value as InsuranceClaim;
            Assert.That(createdClaim.ClaimStatus, Is.EqualTo(ClaimStatus.Pending));
            Assert.That(createdClaim.SettlementAmount, Is.EqualTo(5000));
        }

        [Test]
        public async Task CreateClaim_ReturnsBadRequest_WhenProposalNotApproved()
        {
            var proposal = new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated };
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposal);

            var claim = new InsuranceClaim { ProposalId = 1 };

            var result = await _controller.CreateClaim(claim);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // -------------------- UPDATE --------------------
        [Test]
        public async Task UpdateClaim_ReturnsOk_WhenValid()
        {
            var existing = new InsuranceClaim { ClaimId = 1, ClaimDescription = "Old", SettlementAmount = 1000, ClaimStatus = ClaimStatus.Pending };
            _mockClaimRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockClaimRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var update = new InsuranceClaim { ClaimDescription = "Updated", SettlementAmount = 1500, ClaimStatus = ClaimStatus.Approved };

            var result = await _controller.UpdateClaim(1, update);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            var updatedClaim = ok.Value as InsuranceClaim;
            Assert.That(updatedClaim.ClaimDescription, Is.EqualTo("Updated"));
            Assert.That(updatedClaim.SettlementAmount, Is.EqualTo(1500));
            Assert.That(updatedClaim.ClaimStatus, Is.EqualTo(ClaimStatus.Approved));
        }

        // -------------------- DELETE --------------------
        [Test]
        public async Task DeleteClaim_ReturnsNoContent_WhenExists()
        {
            var claim = new InsuranceClaim { ClaimId = 1 };
            _mockClaimRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(claim);
            _mockClaimRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteClaim(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteClaim_ReturnsNotFound_WhenNotExists()
        {
            _mockClaimRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((InsuranceClaim)null);

            var result = await _controller.DeleteClaim(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}
