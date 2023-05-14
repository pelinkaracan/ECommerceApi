
namespace ECommerceApi.RestApi.Services.Abstractions
{
    // Represents the interface for a log service
    public interface ILogService
    {
        // Creates a log asynchronously based on the provided exception
        public Task CreateLogAsync(Exception excep);
    }
}
