using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;

namespace CLUserManagementAPI.Domain.Interfaces
{
	public interface IUserRepository
	{
		Task<User> GetUserByIdAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task AddUserAsync(User user);
	}
}
