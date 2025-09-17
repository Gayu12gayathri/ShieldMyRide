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
    public class OfficerAssignmentsTests
    {
        private Mock<IOfficerAssignmentRepository> _mockRepository;
        private OfficerAssignmentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IOfficerAssignmentRepository>();
            _controller = new OfficerAssignmentsController(_mockRepository.Object);
        }

        [Test]
        public async Task GetAllAssignments_ReturnsOk_WithAssignments()
        {
            // Arrange
            var assignments = new List<OfficerAssignment>
            {
                new OfficerAssignment { OfficerAssignmentId = 1, Status = "Assigned" },
                new OfficerAssignment { OfficerAssignmentId = 2, Status = "Pending" }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(assignments);

            // Act
            var result = await _controller.GetAllAssignments();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(assignments));
        }

        [Test]
        public async Task GetAssignment_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((OfficerAssignment)null);

            // Act
            var result = await _controller.GetAssignment(99);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAssignment_ReturnsOk_WhenExists()
        {
            // Arrange
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1, Status = "Assigned" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);

            // Act
            var result = await _controller.GetAssignment(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(assignment));
        }

        [Test]
        public async Task CreateAssignment_ReturnsCreatedAtAction()
        {
            // Arrange
            var assignment = new OfficerAssignment
            {
                OfficerAssignmentId = 1,
                Remarks = "New",
                Status = null
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<OfficerAssignment>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateAssignment(assignment);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            var returnedAssignment = createdResult.Value as OfficerAssignment;

            Assert.That(returnedAssignment.Status, Is.EqualTo("Assigned"));
            Assert.That(returnedAssignment.AssignedDate, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task UpdateAssignment_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((OfficerAssignment)null);

            // Act
            var result = await _controller.UpdateAssignment(1, new OfficerAssignment());

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateAssignment_ReturnsOk_WhenExists()
        {
            // Arrange
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1, Status = "Pending", Remarks = "Old" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<OfficerAssignment>()))
                .Returns(Task.CompletedTask);

            var update = new OfficerAssignment { Status = "Approved", Remarks = "Updated" };

            // Act
            var result = await _controller.UpdateAssignment(1, update);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var updatedAssignment = okResult.Value as OfficerAssignment;

            Assert.That(updatedAssignment.Status, Is.EqualTo("Approved"));
            Assert.That(updatedAssignment.Remarks, Is.EqualTo("Updated"));
        }

        [Test]
        public async Task DeleteAssignment_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((OfficerAssignment)null);

            // Act
            var result = await _controller.DeleteAssignment(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteAssignment_ReturnsNoContent_WhenExists()
        {
            // Arrange
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);
            _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAssignment(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
