using ECommerceApi.RestApi.Models.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ECommerceApi.RestApi.Models.Documents
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("items")]
        [JsonPropertyName("items")]
        public List<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id{ get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }


    }


}
