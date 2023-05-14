namespace ECommerceApi.RestApi.Models.Common
{
    // Represents the MongoDB settings required for connecting to a MongoDB database
    public class MongoDbSettings
    {
        // The connection URI of the MongoDB database
        public string ConnectionUri { get; set; } = "";

        // The name of the MongoDB database
        public string DatabaseName { get; set; } = "";
    }
}
