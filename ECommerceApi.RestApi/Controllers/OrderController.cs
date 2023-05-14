using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;

namespace ECommerceApi.RestApi.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly MongoDbService<Order> _mongoDbService;

        public OrderController(IOptions<MongoDbSettings> mongoDbSettings)
        {
            // Initialize the MongoDbService with the provided MongoDbSettings and collection name "Orders"
            _mongoDbService = new MongoDbService<Order>(mongoDbSettings, "Orders");
        }

        [HttpGet]
        public async Task<PagedResult<Order>> GetOrders(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 20,
            [FromQuery(Name = "filter")] string filter = "")
        {
            // Retrieve a paged list of orders from the MongoDbService based on the provided page, pageSize, and filter parameters
            return await _mongoDbService.GetDocumentsAsync(page, pageSize, filter);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            // Generate a new unique identifier (ObjectId) for each order detail item
            foreach (var item in order.OrderDetails)
            {
                item.Id = ObjectId.GenerateNewId().ToString();
            }

            // Create the order document in the MongoDB collection using the MongoDbService
            await _mongoDbService.CreateDocumentAsync(order);

            // Return a 201 Created response with the newly created order and a link to retrieve the orders
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }
    }
}
