using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Helpers;
using ECommerceApi.RestApi.Services.Abstractions;

namespace ECommerceApi.RestApi.Services.Implementations
{
    // Represents a generic MongoDB service
    public class MongoDbService<T>
    {
        // The collection of documents
        public readonly IMongoCollection<T> _collection;

        // Retry policy for handling connection exceptions
        private readonly AsyncRetryPolicy _retryPolicy;

        // Circuit breaker policy for handling connection exceptions
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        // The log service
        private readonly ILogService _logService;

        // Initializes a new instance of the MongoDbService class
        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, string collectionName)
        {
            // Create a MongoDB client and connect to the database
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            // Get the collection of documents
            _collection = database.GetCollection<T>(collectionName);
            _logService = new LogService(mongoDbSettings);

            // Configure the retry policy for handling connection exceptions
            _retryPolicy = Policy
                .Handle<MongoConnectionException>()
                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt =>
                 TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            // Configure the circuit breaker policy for handling connection exceptions
            _circuitBreakerPolicy = Policy
                .Handle<MongoConnectionException>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 2, durationOfBreak: TimeSpan.FromSeconds(30));
        }

        // Retrieves documents asynchronously with pagination and filtering
        public async Task<PagedResult<T>> GetDocumentsAsync(int page, int pageSize, string filter)
        {
            try
            {
                var result = await _retryPolicy
                    .WrapAsync(_circuitBreakerPolicy)
                    .ExecuteAsync(async () =>
                    {
                        var filterDefinition = Builders<T>.Filter.Empty;

                        if (!string.IsNullOrEmpty(filter))
                        {
                            QueryHelper<T> queryHelper = new QueryHelper<T>();
                            // Parse the filter string into a FilterDefinition<T>
                            filterDefinition = queryHelper.ParseFilter(filter);
                        }

                        // Get the total count of documents matching the filter
                        var count = await _collection.CountDocumentsAsync(filterDefinition);

                        // Retrieve the documents based on pagination parameters
                        var products = await _collection.Find(filterDefinition)
                            .Skip((page - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync();

                        // Create a new PagedResult<T> object with the retrieved documents, count, and pagination information
                        return new PagedResult<T>(products, count, page, pageSize);
                    });

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                await _logService.CreateLogAsync(ex);

                // Rethrow exception with a custom message
                throw new Exception($"{ex.Message} error occurred while getting the documents");
            }
        }


        // Retrieves a document by ID asynchronously
        public async Task<T> GetDocumentByIdAsync(string id)
        {
            try
            {
                // Combine retry and circuit breaker policies
                var policyWrap = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

                var result = await policyWrap.ExecuteAsync(async () =>
                {
                    // Create filter to find document with matching ID
                    var filter = Builders<T>.Filter.Eq("Id", id);

                    // Find the document in the collection
                    var data = await _collection.Find(filter).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        // Throw KeyNotFoundException if document is not found
                        throw new KeyNotFoundException($"Document not found with ID: {id}");
                    }

                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                await _logService.CreateLogAsync(ex);

                // Rethrow exception with a custom message
                throw new Exception($"{ex.Message} error occurred while getting the document");
            }
        }

        public async Task CreateDocumentAsync(T document)
        {
            try
            {
                // Combine retry and circuit breaker policies
                var policyWrap = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

                await policyWrap.ExecuteAsync(async () =>
                {
                    // Insert the document into the collection
                    await _collection.InsertOneAsync(document);
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                await _logService.CreateLogAsync(ex);

                // Rethrow exception with a custom message
                throw new Exception($"{ex.Message} error occurred while creating the document");
            }
        }

        public async Task<T> UserExistsAsync(string email, string password)
        {
            try
            {
                // Combine retry and circuit breaker policies
                var policyWrap = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

                var result = await policyWrap.ExecuteAsync(async () =>
                {
                    var filterBuilder = Builders<T>.Filter;

                    // Create filter to find document with matching email and password
                    var filter = filterBuilder.Eq("email", email) & filterBuilder.Eq("password", password);

                    // Find the document in the collection
                    var data = await _collection.Find(filter).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        // Throw KeyNotFoundException if document is not found
                        throw new KeyNotFoundException($"Document not found with Email: {email}");
                    }

                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                await _logService.CreateLogAsync(ex);

                // Rethrow exception with a custom message
                throw new Exception($"{ex.Message} error occurred while checking if the user exists");
            }
        }
    }
}
