using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Services;
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
        [HttpGet("{id}", Name = "Get")]
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] UserModel user)
        {
            // Authenticates a user using username and password.
            user = _userService.Authenticate(user);

            return Ok();
        }
    }
}
