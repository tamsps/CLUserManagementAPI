using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLUserManagementAPI.Application.Interfaces;
using CLUserManagementAPI.Domain.Entities;
using CLUserManagementAPI.Domain.Interfaces;

namespace CLUserManagementAPI.Application.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;


		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task CreateProductAsync(Product product)
		{
			await _productRepository.AddProductAsync(product);
		}

		public async Task UpdateProductAsync(int id, Product product)
		{
			var existingProduct = await _productRepository.GetProductByIdAsync(id);
			if (existingProduct != null)
			{
				existingProduct.Name = product.Name;
				existingProduct.Description = product.Description;
				existingProduct.ImageUrl = product.ImageUrl;
				existingProduct.Price = product.Price;
				existingProduct.Unit = product.Unit;

				await _productRepository.UpdateProductAsync(existingProduct);
			}
		}

		public async Task DeleteProductAsync(int id)
		{
			await _productRepository.DeleteProductAsync(id);
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			return await _productRepository.GetProductByIdAsync(id);
		}

		public async Task<IEnumerable<Product>> GetProductsAsync(string searchTerm = null)
		{
			return await _productRepository.GetProductsAsync(searchTerm);
		}
	}
}
