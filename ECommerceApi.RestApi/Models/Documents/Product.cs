using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.RestApi.Models.Documents
{
    // Represents a product document
    public class Product
    {
        // The unique identifier of the product document
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // The image path or URL of the product
        [BsonElement("image")]
        public string Image { get; set; }

        // The price of the product
        [BsonElement("price")]
        public decimal Price { get; set; }

        // The name of the product
        [BsonElement("name")]
        public string Name { get; set; } = "";

        // The summary or description of the product
        [BsonElement("summary")]
        public string Summary { get; set; } = "";
    }
}
