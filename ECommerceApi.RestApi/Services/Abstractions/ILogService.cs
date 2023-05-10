using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace ECommerceApi.RestApi.Services.Abstractions
{
    public interface ILogService
    {
        public Task CreateLogAsync(Exception excep);
    }
}
