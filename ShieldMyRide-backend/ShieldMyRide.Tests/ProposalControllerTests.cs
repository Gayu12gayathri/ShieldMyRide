using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Controllers;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.Services;

namespace ShieldMyRide.Tests.Controllers
{
    [TestFixture]
    public class ProposalsControllerTests
    {
        private Mock<IProposalRepository> _mockProposalRepo;
        private Mock<IPolicyDocumentRepository> _mockPolicyDocRepo;
        private Mock<IOfficerAssignmentRepository> _mockOfficerRepo;
        private Mock<IPremiumCalculator> _mockPremiumCalc;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<AutoMapper.IMapper> _mockMapper;
        private ProposalsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProposalRepo = new Mock<IProposalRepository>();
            _mockPolicyDocRepo = new Mock<IPolicyDocumentRepository>();
            _mockOfficerRepo = new Mock<IOfficerAssignmentRepository>();
            _mockPremiumCalc = new Mock<IPremiumCalculator>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockMapper = new Mock<AutoMapper.IMapper>();

            _controller = new ProposalsController(
                _mockProposalRepo.Object,
                _mockPolicyDocRepo.Object,
                _mockOfficerRepo.Object,
                _mockPremiumCalc.Object,
                Mock.Of<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>(),
                null,
                _mockPaymentService.Object,
                _mockMapper.Object
            );

            // Mock User Claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("UserId", "1")
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        // -------------------- GET ALL --------------------
        [Test]
        public async Task GetAllProposals_ReturnsOk_WhenProposalsExist()
        {
            var proposals = new List<Proposal>
            {
                new Proposal { ProposalId = 1, VehicleRegNo = "ABC123" },
                new Proposal { ProposalId = 2, VehicleRegNo = "XYZ456" }
            };
            _mockProposalRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(proposals);

            var result = await _controller.GetAllProposals();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var returnProposals = okResult.Value as IEnumerable<Proposal>;
            Assert.That(returnProposals.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllProposals_ReturnsNotFound_WhenNoProposals()
        {
            _mockProposalRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Proposal>());

            var result = await _controller.GetAllProposals();

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("No proposals found."));
        }

        // -------------------- GET BY ID --------------------
        [Test]
        public async Task GetProposal_ReturnsOk_WhenProposalExists()
        {
            var proposal = new Proposal { ProposalId = 1, VehicleRegNo = "ABC123" };
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(proposal);

            var result = await _controller.GetProposal(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var returnProposal = okResult.Value as Proposal;
            Assert.That(returnProposal.VehicleRegNo, Is.EqualTo("ABC123"));
        }

        [Test]
        public async Task GetProposal_ReturnsNotFound_WhenProposalDoesNotExist()
        {
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Proposal)null);

            var result = await _controller.GetProposal(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("Proposal with ID 1 not found."));
        }

        // -------------------- RENEW PROPOSAL --------------------
        [Test]
        public async Task RenewProposal_ReturnsOk_WhenValid()
        {
            var existingProposal = new Proposal
            {
                ProposalId = 1,
                UserId = 1,
                PolicyEndDate = DateTime.UtcNow.AddDays(-1),
                VehicleAge = 2,
                Premium = 1000,
                PolicyId = 1,
                PolicyName = "Car Insurance",
                VehicleRegNo = "ABC123",
                VehicleType = "Car"
            };
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProposal);
            _mockProposalRepo.Setup(r => r.AddAsync(It.IsAny<Proposal>())).Returns(Task.CompletedTask);

            var result = await _controller.RenewProposal(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            dynamic value = okResult.Value;
            Assert.That(value.message.ToString(), Is.EqualTo("Your renewal request has been successfully submitted."));
            Assert.That((int)value.renewalProposal.VehicleAge, Is.EqualTo(3));
        }

        [Test]
        public async Task RenewProposal_ReturnsBadRequest_WhenPolicyStillActive()
        {
            var existingProposal = new Proposal
            {
                ProposalId = 1,
                UserId = 1,
                PolicyEndDate = DateTime.UtcNow.AddDays(10) // Active
            };
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingProposal);

            var result = await _controller.RenewProposal(1);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
