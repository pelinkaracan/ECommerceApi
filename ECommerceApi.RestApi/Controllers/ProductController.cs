using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ECommerceApi.RestApi.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly MongoDbService<Product> _mongoDbService;

        public ProductController(IOptions<MongoDbSettings> mongoDbSettings)
        {
            // Initialize the controller with the MongoDbService for handling product-related operations
            _mongoDbService = new MongoDbService<Product>(mongoDbSettings, "Products");
        }

        [HttpGet("/api/products")]
        public async Task<PagedResult<Product>> GetProducts(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 20,
            [FromQuery(Name = "filter")] string filter = "")
        {
            // Retrieve a paged list of products from the database based on the provided page, pageSize, and filter parameters
            return await _mongoDbService.GetDocumentsAsync(page, pageSize, filter);
        }

        [HttpGet("/api/products/{id}")]
        public async Task<Product> GetProductByIdAsync(string id)
        {
            // Retrieve a product document from the database based on the provided product ID
            return await _mongoDbService.GetDocumentByIdAsync(id);
        }

        [HttpGet("/api/products/{productId}/image")]
        public async Task<IActionResult> GetProductImage(string productId)
        {
            // Retrieve the product document from the database based on the provided product ID
            Product p = await _mongoDbService.GetDocumentByIdAsync(productId);

            var imagePath = p.Image; // Replace with the actual path to your image file

            // Check if the image file exists
            if (!System.IO.File.Exists(imagePath))
            {
                // Return a 404 Not Found response if the image file is not found
                return NotFound();
            }

            // Read the image file bytes
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);

            // Return the image file with the appropriate content type (e.g., "image/jpeg")
            return File(imageBytes, "image/jpeg");
        }
    }
}
