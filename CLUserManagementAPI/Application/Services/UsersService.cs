using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Application.Interfaces;
using CLUserManagementAPI.Domain.Entities;
using CLUserManagementAPI.Domain.Interfaces;
using Org.BouncyCastle.Crypto.Generators;

namespace CLUserManagementAPI.Application.Services
{
	public class UsersService : IUsersService
	{
		private readonly IUserRepository _userRepository;
		private readonly JwtSettings _jwtSettings;

		public UsersService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
		{
			_userRepository = userRepository;
			_jwtSettings = jwtSettings.Value;
		}

		public async Task RegisterUserAsync(User user)
		{
			user.Password = HashPassword(user.Password);
			await _userRepository.AddUserAsync(user);
		}

		public async Task<string> LoginUserAsync(string username, string password)
		{
			var user = await _userRepository.GetUserByUsernameAsync(username);
			if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				return null;
			}
			return GenerateJwtToken(user);
		}

		public async Task LogoutUserAsync()
		{
			// Logic đăng xuất (nếu cần)
		}

		public async Task<User> GetUserInfoAsync(int userId)
		{
			return await _userRepository.GetUserByIdAsync(userId);
		}

		private string HashPassword(string password)
		{
			// Logic hash mật khẩu

			// Mã hóa mật khẩu-*
			return BCrypt.Net.BCrypt.HashPassword(password,12);
		}

	
		private string GenerateJwtToken(User user)
		{
			// Logic tạo JWT token
	

			var claims = new[]
		{
				new Claim(JwtRegisteredClaimNames.Sub, user.Username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
					issuer: _jwtSettings.Issuer,
					audience: _jwtSettings.Audience,
					claims: claims,
					expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes),
					signingCredentials: creds
			);

			// Trả về token dưới dạng chuỗi
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
