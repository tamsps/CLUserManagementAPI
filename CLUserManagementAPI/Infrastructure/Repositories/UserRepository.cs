using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;
using CLUserManagementAPI.Domain.Interfaces;
using CLUserManagementAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Users.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task AddUserAsync(User user)
		{
			var existingUser = await _context.Users
								.FirstOrDefaultAsync(u => u.Username == user.Username);
			if (existingUser != null)
			{
				throw new InvalidOperationException("A user with this username already exists.");
			}

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}
	}
}
