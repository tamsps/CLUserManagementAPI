using CLUserManagementAPI.Application.Interfaces;
using CLUserManagementAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLUserManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{

		private readonly IUsersService _usersService;
		private readonly ILogger<UserController> _logger;

		public UserController(IUsersService usersService, ILogger<UserController> logger)
		{
			_usersService = usersService;
			_logger = logger;
		}

		/// <summary>
		/// Create user
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = await _usersService.GetUserInfoAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}


		/// <summary>
		/// Register user api
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] User user)
		{
			await _usersService.RegisterUserAsync(user);
			return Ok(new { Message = "User registered successfully!" });
		}

		/// <summary>
		/// Login user api
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var token = await _usersService.LoginUserAsync(model.Username, model.Password);
			if (token == null)
			{
				return Unauthorized();
			}
			return Ok(new { Token = token });
		}
		/// <summary>
		/// lohout user
		/// </summary>
		/// <returns></returns>
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			string token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
			await _usersService.LogoutUserAsync();
			return Ok(new { Message = "User logged out successfully!" });
		}

	}
}
