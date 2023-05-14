namespace ECommerceApi.RestApi.Models.Common
{
    // Represents a model for user login information
    public class LoginModel
    {
        // The email of the user
        public string Email { get; set; } = "";

        // The password of the user
        public string Password { get; set; } = "";
    }
}
