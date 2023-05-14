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
using Newtonsoft.Json.Linq;
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
            _mongoDbService = new MongoDbService<User>(mongoDbSettings, "Users");
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _mongoDbService.UserExistsAsync(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(model, Convert.ToInt32(_config["Jwt:ExpirationSecond"]));
            string loginUser = JsonSerializer.Serialize(user);
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
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,model.Email),
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
