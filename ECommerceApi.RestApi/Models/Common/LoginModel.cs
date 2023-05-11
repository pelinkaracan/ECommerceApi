using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.RestApi.Models.Common
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
