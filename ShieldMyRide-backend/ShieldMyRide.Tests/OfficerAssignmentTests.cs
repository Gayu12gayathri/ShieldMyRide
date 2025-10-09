using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Controllers;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using ShieldMyRide.DTOs.OfficerAssignmentDTO;

namespace ShieldMyRide.Tests.Controllers
{
    [TestFixture]
    public class OfficerAssignmentsTests
    {
        private Mock<IOfficerAssignmentRepository> _mockAssignmentRepo;
        private Mock<IUserRepository> _mockUserRepo;
        private OfficerAssignmentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAssignmentRepo = new Mock<IOfficerAssignmentRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _controller = new OfficerAssignmentsController(_mockAssignmentRepo.Object, _mockUserRepo.Object);
        }

        // -------------------- GET ALL --------------------
        [Test]
        public async Task GetAllAssignments_ReturnsOk_WithAssignmentsDTOs()
        {
            var assignments = new List<OfficerAssignment>
            {
                new OfficerAssignment { OfficerAssignmentId = 1, Status = OfficerStatus.Assigned, Officer = new User { FirstName = "John", LastName = "Doe" } },
                new OfficerAssignment { OfficerAssignmentId = 2, Status = OfficerStatus.InProgress }
            };
            _mockAssignmentRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(assignments);

            var result = await _controller.GetAllAssignments();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            Assert.That(okResult.Value, Is.InstanceOf<List<OfficerAssignmentDTO>>());
            var dtoList = okResult.Value as List<OfficerAssignmentDTO>;
            Assert.That(dtoList.Count, Is.EqualTo(2));
            Assert.That(dtoList[0].OfficerAssignmentId, Is.EqualTo(1));
            Assert.That(dtoList[0].OfficerName, Is.EqualTo("John Doe"));
        }

        // -------------------- GET BY ID --------------------
        [Test]
        public async Task GetAssignment_ReturnsNotFound_WhenNotExists()
        {
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync((OfficerAssignment)null);

            var result = await _controller.GetAssignment(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAssignment_ReturnsOk_WithDTO_WhenExists()
        {
            var assignment = new OfficerAssignment
            {
                OfficerAssignmentId = 1,
                Status = OfficerStatus.Assigned,
                Officer = new User { FirstName = "Alice", LastName = "Smith" }
            };
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);

            var result = await _controller.GetAssignment(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;

            Assert.That(okResult.Value, Is.InstanceOf<OfficerAssignmentDTO>());
            var dto = okResult.Value as OfficerAssignmentDTO;

            Assert.That(dto.OfficerAssignmentId, Is.EqualTo(1));
            Assert.That(dto.Status, Is.EqualTo(OfficerStatus.Assigned));
            Assert.That(dto.OfficerName, Is.EqualTo("Alice Smith"));
        }

        // -------------------- CREATE ASSIGNMENT --------------------
        [Test]
        public async Task CreateAssignment_ReturnsBadRequest_WhenAssignedUserIsNotOfficer()
        {
            var assignment = new OfficerAssignment { OfficerId = 1, Remarks = "Test" };
            var user = new User { UserId = 1, Role = "Admin" };

            _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.CreateAssignment(assignment);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value.ToString(), Does.Contain("Only officers can be assigned"));
        }

        [Test]
        public async Task CreateAssignment_ReturnsCreatedAtAction_WhenOfficerIsAssigned()
        {
            var assignment = new OfficerAssignment { OfficerId = 2, Remarks = "Assigned" };
            var officerUser = new User { UserId = 2, Role = "Officer" };

            _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(officerUser);
            _mockAssignmentRepo.Setup(r => r.AddAsync(It.IsAny<OfficerAssignment>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateAssignment(assignment);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            var returnedAssignment = createdResult.Value as OfficerAssignment;

            Assert.That(returnedAssignment.Status, Is.EqualTo(OfficerStatus.Assigned));
            Assert.That(returnedAssignment.AssignedDate, Is.Not.EqualTo(default(DateTime)));
        }

        // -------------------- UPDATE ASSIGNMENT --------------------
        [Test]
        public async Task UpdateAssignment_ReturnsNotFound_WhenNotExists()
        {
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((OfficerAssignment)null);

            var result = await _controller.UpdateAssignment(1, new OfficerAssignment());

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateAssignment_ReturnsOk_WhenExists()
        {
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1, Status = OfficerStatus.InProgress, Remarks = "Old" };
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);
            _mockAssignmentRepo.Setup(r => r.UpdateAsync(It.IsAny<OfficerAssignment>())).Returns(Task.CompletedTask);

            var update = new OfficerAssignment { Status = OfficerStatus.Approved, Remarks = "Updated" };

            var result = await _controller.UpdateAssignment(1, update);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var updatedAssignment = okResult.Value as OfficerAssignment;

            Assert.That(updatedAssignment.Status, Is.EqualTo(OfficerStatus.Approved));
            Assert.That(updatedAssignment.Remarks, Is.EqualTo("Updated"));
        }

        // -------------------- DELETE ASSIGNMENT --------------------
        [Test]
        public async Task DeleteAssignment_ReturnsNotFound_WhenNotExists()
        {
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((OfficerAssignment)null);

            var result = await _controller.DeleteAssignment(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteAssignment_ReturnsNoContent_WhenExists()
        {
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1 };
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);
            _mockAssignmentRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteAssignment(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        // -------------------- EXTRA TEST CASES --------------------
        [Test]
        public async Task CreateAssignment_DefaultsStatusToAssigned_WhenInvalidStatusProvided()
        {
            var assignment = new OfficerAssignment { OfficerId = 2, Status = (OfficerStatus)999, Remarks = "Invalid Status" };
            var officerUser = new User { UserId = 2, Role = "Officer" };

            _mockUserRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(officerUser);
            _mockAssignmentRepo.Setup(r => r.AddAsync(It.IsAny<OfficerAssignment>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateAssignment(assignment);

            var createdResult = result as CreatedAtActionResult;
            var returnedAssignment = createdResult.Value as OfficerAssignment;

            Assert.That(returnedAssignment.Status, Is.EqualTo(OfficerStatus.Assigned)); // should default
        }

        [Test]
        public async Task UpdateAssignment_DoesNotChangeStatus_WhenInvalidStatusProvided()
        {
            var assignment = new OfficerAssignment { OfficerAssignmentId = 1, Status = OfficerStatus.Assigned, Remarks = "Old" };
            _mockAssignmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(assignment);

            var update = new OfficerAssignment { Status = (OfficerStatus)999, Remarks = "Updated" };

            var result = await _controller.UpdateAssignment(1, update);

            var okResult = result as OkObjectResult;
            var updatedAssignment = okResult.Value as OfficerAssignment;

            Assert.That(updatedAssignment.Status, Is.EqualTo(OfficerStatus.Assigned)); // unchanged
            Assert.That(updatedAssignment.Remarks, Is.EqualTo("Updated"));
        }
    }
}
