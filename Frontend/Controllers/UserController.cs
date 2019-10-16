using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDbContext<UserModel> _usersDbContext;
        private readonly MongoDBAppSettings _mongoDbSettings;

        // Userservice needed for authentication.
        private readonly IUserService _userService;

        public UserController(IOptions<MongoDBAppSettings> mongoDbSettings, IOptions<SecretSettings> secretSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _usersDbContext = new UsersDbContext(_mongoDbSettings);
            _userService = new UserService(secretSettings.Value, mongoDbSettings.Value);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public UserModel Get(string id)
        {
            var user = _usersDbContext.Get(id);
            return user;
        }

        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] UserModel userModel)
        {
            userModel = _usersDbContext.Post(userModel);
            return Ok(userModel);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserModel userModel)
        {
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult Authenticated()
        {
            return Ok(new Response() { Status = "Success", Message = "Authenticated" });
        }

        [HttpPost("[action]")]
        public IActionResult Authenticate([FromBody] UserModel user)
        {
            // Authenticates a user using username and password.
            user = _userService.Authenticate(user);

            if (user != null)
            {
                Response.Cookies.Append(
                    "Auth",
                    user.Token,
                    new CookieOptions()
                    {
                        Path = "/",
                        Secure = true,
                        HttpOnly = false
                    });

                return Ok(new Response { Status = "Success", Message = user.Token });
            }
            else
            {
                //return Ok("Invalid username or password");
                return Ok(new Response { Status = "Failure", Message = "Invalid Username or Password" });
            }

        }
    }
}
