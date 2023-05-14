using Microsoft.AspNetCore.Mvc;
using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using Microsoft.Extensions.Options;
using ECommerceApi.RestApi.Services.Implementations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerceApi.RestApi.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoDbService<User> _mongoDbService;
        private readonly IConfiguration _config;

        public UserController(IOptions<MongoDbSettings> mongoDbSettings, IConfiguration config)
        {
            // Initialize the controller with the MongoDbService for handling user-related operations
            _mongoDbService = new MongoDbService<User>(mongoDbSettings, "Users");
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Check if the user exists in the database based on the provided email and password
            var user = await _mongoDbService.UserExistsAsync(model.Email, model.Password);

            if (user == null)
            {
                // Return 401 Unauthorized if the user does not exist or the password is incorrect
                return Unauthorized();
            }

            // Generate a JWT token for the authenticated user
            var token = GenerateToken(model, Convert.ToInt32(_config["Jwt:ExpirationSecond"]));

            // Serialize the user object to JSON string
            string loginUser = JsonSerializer.Serialize(user);

            // Return the JWT token and serialized user object in the response
            return Ok(new { Token = token, User = loginUser });
        }

        private string GenerateToken(LoginModel model, int expirationSecond)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Compute the hash of the entered password
                var enteredPasswordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                var securityKey = new SymmetricSecurityKey(enteredPasswordHash);
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Set the claims for the JWT token 
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,model.Email),
                };

                // Create a JWT token with the configured issuer, audience, claims, expiration, and signing credentials
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddSeconds(expirationSecond),
                    signingCredentials: credentials);

                // Write the JWT token as a string
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
