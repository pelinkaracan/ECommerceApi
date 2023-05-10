using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using System;
using ECommerceApi.RestApi.Models.Common;
using Microsoft.AspNetCore.Identity;
using ECommerceApi.RestApi.Helpers;
using ECommerceApi.RestApi.Models.Documents;
using System.Web.Http;
using ECommerceApi.RestApi.Services.Abstractions;

namespace ECommerceApi.RestApi.Services.Implementations
{
    public class MongoDbService<T>
    {
        public readonly IMongoCollection<T> _collection;

        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ILogService _logService;
        //private readonly UserManager<ApplicationUser> _userManager;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, string collectionName)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUri);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
            _logService = new LogService(mongoDbSettings);
            //_userManager = userManager;

            _retryPolicy = Policy
                .Handle<MongoConnectionException>()
                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt =>
                 TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy
                .Handle<MongoConnectionException>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 2, durationOfBreak: TimeSpan.FromSeconds(30));
        }

        public async Task<IEnumerable<T>> GetDocumentsAsync(int page, int pageSize, string filter)
        {
            try
            {
                var result = await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .ExecuteAsync(async () =>
                {
                    var filterDefinition = Builders<T>.Filter.Empty;

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        QueryHelper<T> queryHelper = new QueryHelper<T>();
                        // Parse the filter string into a FilterDefinition<T>
                        filterDefinition = queryHelper.ParseFilter(filter);
                    }

                    var count = await _collection.CountDocumentsAsync(filterDefinition);
                    var products = await _collection.Find(filterDefinition)
                        .Skip((page - 1) * pageSize)
                        .Limit(pageSize)
                        .ToListAsync();

                    return new PagedResult<T>(products, count, page, pageSize).Items;
                });

                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex);
                throw new Exception($"{ex.Message} error is getting");
            }

        }

        public async Task<T> GetDocumentByIdAsync(string id)
        {
            try
            {
                var policyWrap = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

                var result = await policyWrap.ExecuteAsync(async () =>
                {
                    var filter = Builders<T>.Filter.Eq("Id", id);

                    var data = await _collection.Find(filter).FirstOrDefaultAsync();

                    if (data == null)
                    {
                        throw new KeyNotFoundException($"Document not found with ID: {id}");
                    }

                    return data;
                });

                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex);
                throw new Exception($"{ex.Message} error is getting");
            }
           
        }

        public async Task CreateDocumentAsync(T document)
        {
            try
            {
                var policyWrap = Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy);

                await policyWrap.ExecuteAsync(async () =>
                {
                    await _collection.InsertOneAsync(document);

                });
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex);
                throw new Exception($"{ex.Message} error is getting");
            }
          
        }


        //public async Task<bool> UserExistsAsync(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    return user != null;
        //}

    }
}
