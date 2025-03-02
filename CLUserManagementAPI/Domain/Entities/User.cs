using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUserManagementAPI.Domain.Entities
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		public string PasswordHash { get; set; }
	}
}
