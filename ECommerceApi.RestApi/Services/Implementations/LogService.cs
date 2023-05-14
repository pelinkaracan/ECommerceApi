using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using ECommerceApi.RestApi.Services.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Web.Http;

namespace ECommerceApi.RestApi.Services.Implementations
{
    // Represents a log service implementation
    public class LogService : ILogService
    {
        // The collection of log documents
        public readonly IMongoCollection<Log> _logs;

        // Initializes a new instance of the LogService class
        public LogService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            // Create a MongoDB client and connect to the database
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            // Get the collection of log documents
            _logs = database.GetCollection<Log>("Logs");
        }

        // Creates a log asynchronously based on the provided exception
        public async Task CreateLogAsync(Exception excep)
        {
            // Retrieve the status code from the exception
            int statusCode;

            if (excep is HttpResponseException httpResponseException)
            {
                statusCode = (int)httpResponseException.Response.StatusCode;
            }
            else
            {
                // If the exception is not an HttpResponseException
                //TODO: use a generic status code
                statusCode = 500;
            }

            // Create a new log instance
            Log log = new Log();
            log.CreatedDate = DateTime.Now;
            log.Message = excep.Message;
            log.StatusCode = statusCode;

            // Insert the log document into the collection
            await _logs.InsertOneAsync(log);
        }
    }
}
