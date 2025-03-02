using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;
using CLUserManagementAPI.Domain.Interfaces;
using CLUserManagementAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CLUserManagementAPI.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			return await _context.Products.FindAsync(id);
		}

		public async Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null)
		{
			var query = _context.Products.AsQueryable();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(p => p.Name.Contains(searchTerm));
			}

			return await query.ToListAsync();
		}

		public async Task AddProductAsync(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
		}
	}
}
