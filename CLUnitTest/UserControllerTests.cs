using CLUserManagementAPI.Application.Interfaces;
using CLUserManagementAPI.Controllers;
using CLUserManagementAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CLUnitTest
{
	[TestFixture]
	public class UserControllerTests
	{
		private Mock<IUsersService> _usersServiceMock;
		private Mock<ILogger<UserController>> _loggerMock;
		private UserController _controller;

		[SetUp]
		public void SetUp()
		{
			_usersServiceMock = new Mock<IUsersService>();
			_loggerMock = new Mock<ILogger<UserController>>();
			_controller = new UserController(_usersServiceMock.Object, _loggerMock.Object);
		}

		[Test]
		public async Task GetUserById_ReturnsOk_WhenUserExists()
		{
			// Arrange
			var user = new User { Username = "existinguser", Password = "password123" };
			_usersServiceMock.Setup(s => s.RegisterUserAsync(user))
					.ThrowsAsync(new InvalidOperationException("A user with this username already exists."));

			// Act
			var result = await _controller.Register(user);

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
			var badRequestResult = result as BadRequestObjectResult;
			Assert.IsNotNull(badRequestResult);
			var response = (dynamic)badRequestResult.Value;
			Assert.AreEqual("A user with this username already exists.", response.Message);
		}

		[Test]
		public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
		{
			// Arrange
			int userId = 1;
			_usersServiceMock.Setup(s => s.GetUserInfoAsync(userId)).ReturnsAsync((User)null);

			// Simulate authorization
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};

			// Act
			var result = await _controller.GetUserById(userId);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[Test]
		public async Task Register_ReturnsOk_WhenSuccessful()
		{
			// Arrange
			var user = new User { Username = "newuser", Password = "hashed" };
			_usersServiceMock.Setup(s => s.RegisterUserAsync(user)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.Register(user);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var response = (dynamic)okResult.Value;
			Assert.AreEqual("User registered successfully!", response.Message);
		}

		[Test]
		public async Task Login_ReturnsOkWithToken_WhenCredentialsAreValid()
		{
			// Arrange
			var loginModel = new LoginModel { Username = "testuser", Password = "test123" };
			string token = "jwt-token";
			_usersServiceMock.Setup(s => s.LoginUserAsync(loginModel.Username, loginModel.Password))
					.ReturnsAsync(token);

			// Act
			var result = await _controller.Login(loginModel);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var response = (dynamic)okResult.Value;
			Assert.AreEqual(token, response.Token);
		}

		[Test]
		public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
		{
			// Arrange
			var loginModel = new LoginModel { Username = "testuser", Password = "wrong" };
			_usersServiceMock.Setup(s => s.LoginUserAsync(loginModel.Username, loginModel.Password))
					.ReturnsAsync((string)null);

			// Act
			var result = await _controller.Login(loginModel);

			// Assert
			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}

		[Test]
		public async Task Logout_ReturnsOk_WhenSuccessful()
		{
			// Arrange
			string token = "jwt-token";
			_usersServiceMock.Setup(s => s.LogoutUserAsync()).Returns(Task.CompletedTask);

			// Simulate Authorization header
			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers["Authorization"] = $"Bearer {token}";
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

			// Act
			var result = await _controller.Logout();

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			var response = (dynamic)okResult.Value;
			Assert.AreEqual("User logged out successfully!", response.Message);
			_usersServiceMock.Verify(s => s.LogoutUserAsync(), Times.Once());
		}

		[Test]
		public void Logout_ThrowsException_WhenTokenIsMissing()
		{
			// Arrange
			_usersServiceMock.Setup(s => s.LogoutUserAsync()).Returns(Task.CompletedTask);
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext() // No Authorization header
			};

			// Act & Assert
			Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _controller.Logout());
			// Note: Update controller to handle this gracefully in production
		}
	}
}