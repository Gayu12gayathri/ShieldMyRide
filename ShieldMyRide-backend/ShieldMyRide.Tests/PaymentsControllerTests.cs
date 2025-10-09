using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Controllers;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.DTOs.PaymentDTO;

namespace ShieldMyRide.Tests.Controllers
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IPaymentRepository> _mockPaymentRepo;
        private Mock<IProposalRepository> _mockProposalRepo;
        private Mock<IClaimRepository> _mockClaimRepo;
        private Mock<IQuoteRepository> _mockQuoteRepo;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPaymentRepo = new Mock<IPaymentRepository>();
            _mockProposalRepo = new Mock<IProposalRepository>();
            _mockClaimRepo = new Mock<IClaimRepository>();
            _mockQuoteRepo = new Mock<IQuoteRepository>();

            _controller = new PaymentsController(
                _mockPaymentRepo.Object,
                _mockProposalRepo.Object,
                _mockClaimRepo.Object,
                _mockQuoteRepo.Object
            );
        }

        // -------------------- GET ALL --------------------
        [Test]
        public async Task GetAllPayments_ReturnsOk_WithBalanceAndStatus()
        {
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, ProposalID = 1, AmountPaid = 500, ForClaim = false },
                new Payment { PaymentId = 2, ProposalID = 1, AmountPaid = 500, ForClaim = false }
            };
            _mockPaymentRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(payments);
            _mockPaymentRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(payments);
            _mockQuoteRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new Quote { ProposalId = 1, PremiumAmount = 1000 });
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated });

            var result = await _controller.GetAllPayments();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        // -------------------- GET BY ID --------------------
        [Test]
        public async Task GetPayment_ReturnsOk_WhenExists()
        {
            var payment = new Payment { PaymentId = 1, ProposalID = 1, AmountPaid = 500, ForClaim = false };
            _mockPaymentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payment);
            _mockPaymentRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new List<Payment> { payment });
            _mockQuoteRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new Quote { ProposalId = 1, PremiumAmount = 1000 });
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated });

            var result = await _controller.GetPayment(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetPayment_ReturnsNotFound_WhenNotExists()
        {
            _mockPaymentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Payment)null);

            var result = await _controller.GetPayment(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        // -------------------- CREATE PAYMENT --------------------
        [Test]
        public async Task CreatePayment_ReturnsOk_WhenValid()
        {
            var payment = new Payment { PaymentId = 1, ProposalID = 1, AmountPaid = 500, ForClaim = false };
            _mockPaymentRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new List<Payment>());
            _mockQuoteRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new Quote { ProposalId = 1, PremiumAmount = 1000 });
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated });
            _mockPaymentRepo.Setup(r => r.AddAsync(payment)).Returns(Task.CompletedTask);
            _mockProposalRepo.Setup(r => r.UpdateAsync(It.IsAny<Proposal>())).Returns(Task.CompletedTask);

            var result = await _controller.CreatePayment(payment);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        // -------------------- UPDATE PAYMENT --------------------
        [Test]
        public async Task UpdatePayment_ReturnsOk_WhenValid()
        {
            var payment = new Payment { PaymentId = 1, ProposalID = 1, AmountPaid = 500, ForClaim = false };
            _mockPaymentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payment);
            _mockPaymentRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new List<Payment> { payment });
            _mockQuoteRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new Quote { ProposalId = 1, PremiumAmount = 1000 });
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated });
            _mockPaymentRepo.Setup(r => r.UpdateAsync(payment)).Returns(Task.CompletedTask);

            var dto = new UpdatePaymentDTO { AmountPaid = 200 };

            var result = await _controller.UpdatePayment(1, dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        // -------------------- DELETE PAYMENT --------------------
        [Test]
        public async Task DeletePayment_ReturnsOk_WhenExists()
        {
            var payment = new Payment { PaymentId = 1, ProposalID = 1, AmountPaid = 500, ForClaim = false };
            _mockPaymentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payment);
            _mockPaymentRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new List<Payment>());
            _mockQuoteRepo.Setup(r => r.GetByProposalIdAsync(1)).ReturnsAsync(new Quote { ProposalId = 1, PremiumAmount = 1000 });
            _mockProposalRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Proposal { ProposalId = 1, ProposalStatus = ProposalStatus.QuoteGenerated });
            _mockPaymentRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeletePayment(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
