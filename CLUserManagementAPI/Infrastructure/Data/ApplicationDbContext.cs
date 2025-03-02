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
				entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
				entity.Property(u => u.Email).IsRequired();
				entity.Property(u => u.Password).IsRequired();
			});

			builder.Entity<User>()
									.HasIndex(u => u.Username)
									.IsUnique();

			builder.Entity<User>()
											.HasMany(u => u.Products) // Một User có nhiều Product
											.WithOne(p => p.user)     // Một Product thuộc về một User
											.OnDelete(DeleteBehavior.Cascade); // Xóa Product kh


			// Ensure Id is an identity column
			builder.Entity<User>()
					.Property(u => u.Id)
					.ValueGeneratedOnAdd(); 

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
