using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;

namespace CLUserManagementAPI.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Cấu hình entity User
			builder.Entity<User>(entity =>
			{
				entity.HasKey(u => u.Id);
				entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
				entity.Property(u => u.Email).IsRequired();
				entity.Property(u => u.PasswordHash).IsRequired();
			});

			// Cấu hình entity Product
			builder.Entity<Product>(entity =>
			{
				entity.HasKey(p => p.Id);
				entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
				entity.Property(p => p.Description).HasMaxLength(500);
				entity.Property(p => p.ImageUrl).IsRequired();
				entity.Property(p => p.Price).IsRequired();
				entity.Property(p => p.Unit).IsRequired().HasMaxLength(20);
			});
		}
	}

}
