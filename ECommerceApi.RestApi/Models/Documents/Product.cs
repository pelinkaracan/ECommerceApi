using ECommerceApi.RestApi.Models.Common;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.RestApi.Models.Documents
{
    public class Product 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

    }
}
