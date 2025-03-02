using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;

namespace CLUserManagementAPI.Application.Interfaces
{
	public interface IUsersService
	{
		Task RegisterUserAsync(User user);
		Task<string> LoginUserAsync(string username, string password);
		Task LogoutUserAsync();
		Task<User> GetUserInfoAsync(int userId);
	}
}
