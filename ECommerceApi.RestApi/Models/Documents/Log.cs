using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.RestApi.Models.Documents
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate{ get; set; }

        [BsonElement("statusCode")]
        public int StatusCode { get; set; }

        [BsonElement("message")]
        public string Message { get; set; }
    }
}
