using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerceApi.RestApi.Controllers
{
    [Controller]
    [Route("api /[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly MongoDbService<Product> _mongoDbService;

        public ProductController(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = new MongoDbService<Product>(mongoDbSettings, "Products");
        }

        [HttpGet("/api/products")]
        public async Task<IEnumerable<Product>> GetProducts(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 20,
            [FromQuery(Name = "filter")] string filter = "") 
        {
            return await  _mongoDbService.GetDocumentsAsync(page, pageSize, filter);
        }

        [HttpGet("/api/products/{id}")]
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _mongoDbService.GetDocumentByIdAsync(id);
        }
    }
}
