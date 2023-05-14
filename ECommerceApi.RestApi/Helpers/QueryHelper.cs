using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApi.RestApi.Helpers
{
    public class QueryHelper<T>
    {
        public FilterDefinition<T> ParseFilter(string filter)
        {
            // Parse the filter string into a FilterDefinition<T>
            // Here's an example => filter=Name contains 'Product'
            var parts = filter.Split(" ");
            var field = parts[0];
            var op = parts[1];
            var value = parts[2];

            switch (op)
            {
                case "contains":
                    return Builders<T>.Filter.Regex(field, new BsonRegularExpression(value, "i"));
                default:
                    throw new ArgumentException($"Invalid operator: {op}");
            }
        }
    }
}
