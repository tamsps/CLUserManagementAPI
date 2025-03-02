using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;

namespace CLUserManagementAPI.Domain.Interfaces
{
	public interface IProductRepository
	{
		Task<Product> GetProductByIdAsync(int id);
		Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null);
		Task AddProductAsync(Product product);
		Task UpdateProductAsync(Product product);
		Task DeleteProductAsync(int id);
	}
}
