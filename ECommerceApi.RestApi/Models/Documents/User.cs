using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.RestApi.Models.Documents
{
    // Represents a user document
    public class User
    {
        // The unique identifier of the user document
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // The first name of the user
        [BsonElement("firstName")]
        public string FirstName { get; set; } = "";

        // The last name of the user
        [BsonElement("lastName")]
        public string LastName { get; set; } = "";

        // The email of the user
        [BsonElement("email")]
        public string Email { get; set; } = "";

        // The password of the user
        [BsonElement("password")]
        public string Password { get; set; } = "";
    }
}
