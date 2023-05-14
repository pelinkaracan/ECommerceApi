using Amazon.Runtime.Internal;
using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Abstractions;
using ECommerceApi.RestApi.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            _mongoDbService = new MongoDbService<Order>(mongoDbSettings, "Orders");
        }

        [HttpGet]
        public async Task<PagedResult<Order>> GetOrders(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 20,
            [FromQuery(Name = "filter")] string filter = "")
        {
            return await _mongoDbService.GetDocumentsAsync(page, pageSize, filter);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            foreach (var item in order.OrderDetails)
            {
                item.Id = ObjectId.GenerateNewId().ToString();
            }
            await _mongoDbService.CreateDocumentAsync(order);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);

        }

    }
}
