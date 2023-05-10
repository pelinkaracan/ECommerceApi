using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Web.Http;

namespace ECommerceApi.RestApi.Services.Implementations
{
    public class LogService : ILogService
    {

        public readonly IMongoCollection<Log> _logs;


        public LogService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _logs = database.GetCollection<Log>("Logs");
        }

        public async Task CreateLogAsync(Exception excep)
        {
            // Retrieve the status code
            int statusCode;

            if (excep is HttpResponseException httpResponseException)
            {
                statusCode = (int)httpResponseException.Response.StatusCode;
            }
            else
            {
                //TODO : generic status code 
                statusCode = 500;
            }
            Log log = new Log();
            log.CreatedDate = DateTime.Now;
            log.Message = excep.Message;
            log.StatusCode = statusCode;
            await _logs.InsertOneAsync(log);
        }

    }
}
