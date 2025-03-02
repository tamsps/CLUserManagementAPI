using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLUserManagementAPI.Domain.Entities
{
	public class Product
	{
		public int Id { get; set; }

		[Required]
		[StringLength(500)]
		public string Name { get; set; }

		public User? user { get; set; } // Cho phép UserId là null

		[StringLength(500)]
		public string Description { get; set; }

		public string ImageUrl { get; set; }

		[Range(0.01, 1000000)]
		public decimal Price { get; set; }

		[StringLength(20)]
		public string Unit { get; set; }
	}
}
