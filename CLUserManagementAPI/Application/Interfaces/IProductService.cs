using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Domain.Entities;

namespace CLUserManagementAPI.Application.Interfaces
{
	public interface IProductService
	{
		Task CreateProductAsync(Product product);
		Task UpdateProductAsync(int id, Product product);
		Task DeleteProductAsync(int id);
		Task<Product> GetProductByIdAsync(int id);
		Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null);
	}
}
