using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShieldMyRide.Authentication;
using ShieldMyRide.Controllers.AuthControllers;
using ShieldMyRide.DTOs.UsersDTO;

namespace ShieldMyRide.Tests
{
    [TestFixture]
    public class OfficerControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _mockMapper = new Mock<IMapper>();
            _controller = new UserController(_mockUserManager.Object, _mockMapper.Object);
        }

        [Test]
        public void SanityCheck()
        {
            Assert.That(1, Is.EqualTo(1));
            Assert.That("hello", Is.InstanceOf<string>());
        }

        [Test]
        public async Task GetCustomer_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            string testUserId = "123";
            _mockUserManager.Setup(u => u.FindByIdAsync(testUserId))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.GetCustomer(testUserId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetCustomer_ReturnsOk_WhenUserExists()
        {
            // Arrange
            string testUserId = "456";
            var user = new ApplicationUser { Id = testUserId, UserName = "testuser" };

            _mockUserManager.Setup(u => u.FindByIdAsync(testUserId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<CustomerDTO>(user))
                .Returns(new CustomerDTO { Id = testUserId, UserName = "testuser" });

            // Act
            var result = await _controller.GetCustomer(testUserId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            var dto = okResult.Value as CustomerDTO;
            Assert.That(dto.Id, Is.EqualTo(testUserId));
        }
    }
}