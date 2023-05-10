using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ECommerceApi.RestApi.Models.Common;
using ECommerceApi.RestApi.Models.Documents;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ECommerceApi.RestApi.Services.Implementations;

namespace ECommerceApi.RestApi.Controllers
{
    //[Controller]
    //[Route("api /[controller]")]
    public class UserController 
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MongoDbService<User> _mongoDbService;

        public UserController(IOptions<MongoDbSettings> mongoDbSettings, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mongoDbService = new MongoDbService<User>(mongoDbSettings, "Users");

        }

        //    [HttpPost("login")]
        //    public async Task<IActionResult> LoginAsync(string email, string password)
        //    {
        //        // Check if the user exists in the database
        //        var userExists = await _mongoDbService.UserExistsAsync(email);
        //        if (!userExists)
        //        {
        //            return NotFound("User not found");
        //        }

        //        // Attempt to sign in the user
        //        var user = await _userManager.FindByEmailAsync(email);
        //        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest("Invalid login credentials");
        //        }

        //        return Ok();
        //    }

        //    [HttpPost("logout")]
        //    public async Task<IActionResult> LogoutAsync()
        //    {
        //        await _signInManager.SignOutAsync();

        //        return Ok();
        //    }
    }
}
