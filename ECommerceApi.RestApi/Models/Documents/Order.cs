using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ECommerceApi.RestApi.Models.Documents
{
    // Represents an order document
    public class Order
    {
        // The unique identifier of the order document
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // The date when the order was placed
        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        // The user ID associated with the order
        [BsonElement("userId")]
        public string UserId { get; set; }

        // The list of order details
        [BsonElement("items")]
        [JsonPropertyName("items")]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    // Represents an order detail within an order document
    public class OrderDetail
    {
        // The unique identifier of the order detail
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // The product ID associated with the order detail
        [BsonElement("productId")]
        public string ProductId { get; set; }

        // The quantity of the product within the order detail
        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}
