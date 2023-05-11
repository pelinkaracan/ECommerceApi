using Microsoft.AspNetCore.Mvc;
using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using Microsoft.Extensions.Options;
using ECommerceApi.RestApi.Services.Implementations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MongoDB.Driver.Linq;

namespace ECommerceApi.RestApi.Controllers
{
    [Controller]
    [Route("api /[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MongoDbService<User> _mongoDbService;
        private readonly IConfiguration _config;

        public UserController(IOptions<MongoDbSettings> mongoDbSettings, IConfiguration config)
        {
            _mongoDbService = new MongoDbService<User>(mongoDbSettings, "Users");
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _mongoDbService.UserExistsAsync(model.Email,model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user, Convert.ToInt32(_config["Jwt:ExpirationSecond"]));
            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] User user)
        {
            // Perform any necessary cleanup or session management tasks
            // For example, you could invalidate the user's token or remove it from the client-side storage
            var token = GenerateToken(user, 10);
            return Ok(new { Token = token, Message = "Logout successful" });
        }

        private string GenerateToken(User user,int expirationSecond)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Compute the hash of the entered password
                var enteredPasswordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                var securityKey = new SymmetricSecurityKey(enteredPasswordHash);
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
            };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddSeconds(expirationSecond),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }

}