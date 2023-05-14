using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.RestApi.Models.Documents
{
    // Represents a log document
    public class Log
    {
        // The unique identifier of the log document
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // The date and time when the log was created
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        // The status code associated with the log
        [BsonElement("statusCode")]
        public int StatusCode { get; set; }

        // The log message
        [BsonElement("message")]
        public string Message { get; set; } = "";
    }
}
