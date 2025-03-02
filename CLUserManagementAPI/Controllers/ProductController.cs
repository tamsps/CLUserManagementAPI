using CLUserManagementAPI.Application.Interfaces;
using CLUserManagementAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CLUserManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] Product product)
		{
			await _productService.CreateProductAsync(product);
			return Ok(new { Message = "Product created successfully!" });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
		{
			await _productService.UpdateProductAsync(id, product);
			return Ok(new { Message = "Product updated successfully!" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			await _productService.DeleteProductAsync(id);
			return Ok(new { Message = "Product deleted successfully!" });
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts([FromQuery] string searchTerm = null)
		{
			var products = await _productService.GetProductsAsync(searchTerm);
			return Ok(products);
		}

	}
}
