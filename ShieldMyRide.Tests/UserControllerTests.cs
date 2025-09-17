using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Context;
using ShieldMyRide.Controllers.AuthControllers;
using ShieldMyRide.DTOs.ClaimDTO;
using ShieldMyRide.DTOs.PolicyDTO;
using ShieldMyRide.DTOs.ProposalDTO;
using ShieldMyRide.DTOs.QuoteDTO;
using ShieldMyRide.Models;

namespace ShieldMyRide.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private MyDBContext _context;
        private Mock<IMapper> _mockMapper;
        private CustomerController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MyDBContext(options);
            _mockMapper = new Mock<IMapper>();
            _controller = new CustomerController(_context, _mockMapper.Object);

            // Mock user claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("UserId", "1"),
                new Claim(ClaimTypes.Role, "User")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TearDown]
        public void TearDown() => _context?.Dispose();

        [Test]
        public async Task GetUserProposals_ReturnsOk_WithMappedProposals()
        {
            var proposal = new Proposal
            {
                ProposalId = 1,
                UserId = 1,
                PolicyId = 1,                       // required
                VehicleType = "Car",
                VehicleRegNo = "TN09AB1234",        // required
                Premium = 5000,
                ProposalStatus = ProposalStatus.Submitted,
                CreatedAt = DateTime.UtcNow
            };

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<IEnumerable<ProposalDTO>>(It.IsAny<IEnumerable<Proposal>>()))
                .Returns(new List<ProposalDTO> { new ProposalDTO { Id = 1, VehicleType = "Car", Premium = 5000, Status = "Pending" } });

            var result = await _controller.GetUserProposals();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserClaims_ReturnsOk_WithMappedClaims()
        {
            // Arrange
            var claim = new InsuranceClaim
            {
                ClaimId = 1,
                UserId = 1,
                ClaimDescription = "Accident on highway",
                ClaimStatus = "Pending"
            };

            _context.InsuranceClaims.Add(claim);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<IEnumerable<ClaimGetDTO>>(It.IsAny<IEnumerable<InsuranceClaim>>()))
                .Returns(new List<ClaimGetDTO> { new ClaimGetDTO { ClaimId = 1 } });

            // Act
            var result = await _controller.GetUserClaims();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var returnedClaims = okResult.Value as IEnumerable<ClaimGetDTO>;
            Assert.That(returnedClaims.Count(), Is.EqualTo(1));
            Assert.That(returnedClaims.First().ClaimId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserQuotes_ReturnsOk_WithMappedQuotes()
        {
            var proposal = new Proposal
            {
                ProposalId = 10,
                UserId = 1,
                PolicyId = 1,                       // required
                VehicleType = "Bike",
                VehicleRegNo = "TN10XY1234",        // required
                Premium = 2000,
                ProposalStatus = ProposalStatus.Submitted,
                CreatedAt = DateTime.UtcNow
            };

            var quote = new Quote
            {
                QuoteId = 5,
                CoverageDetails = "Full coverage",
                Proposal = new List<Proposal> { proposal }
            };

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<IEnumerable<QuoteGetDTO>>(It.IsAny<IEnumerable<Quote>>()))
                .Returns(new List<QuoteGetDTO> { new QuoteGetDTO { QuoteId = 5} });

            var result = await _controller.GetUserQuotes();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserPolicies_ReturnsOk_WithMappedPolicies()
        {
            var proposal = new Proposal
            {
                ProposalId = 100,
                UserId = 1,
                PolicyId = 200,                     // required
                VehicleType = "SUV",
                VehicleRegNo = "TN11ZZ5678",        // required
                Premium = 8000,
                ProposalStatus = ProposalStatus.Submitted,
                CreatedAt = DateTime.UtcNow
            };

            var policy = new Policy
            {
                PolicyId = 200,
                PolicyName = "Comprehensive Cover",
                CoverageType = "Full",              // required
                Proposals = new List<Proposal> { proposal }
            };

            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            _mockMapper.Setup(m => m.Map<IEnumerable<PolicyGetDTO>>(It.IsAny<IEnumerable<Policy>>()))
                .Returns(new List<PolicyGetDTO> { new PolicyGetDTO { PolicyId = 200, PolicyName = "Comprehensive Cover" } });

            var result = await _controller.GetUserPolicies();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
