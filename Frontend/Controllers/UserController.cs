﻿using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersDbContext _usersDbContext;

        // Userservice needed for authentication.
        private readonly IUserService _userService;

        public UserController(IUsersDbContext dbContext, IUserService userService)
        {
            _usersDbContext = dbContext;
            _userService = userService;
        }

        // GET: api/User/
        [HttpGet()]
        public ActionResult<List<UserModel>> Get()
        {
            var user = _usersDbContext.Get();
            return Ok(user);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<UserModel> Get(string id)
        {
            var user = _usersDbContext.Get(id);
            user.Password = null;
            return Ok(user);
        }

        // GET: api/User/FromToken
        [HttpGet("[action]")]
        public ActionResult<UserModel> FromToken()
        {
            var user = User.Identity as ClaimsIdentity;
            var userId = user.FindFirst(ClaimTypes.Name)?.Value;

            if (userId != null)
            {
                return Ok(_usersDbContext.Get(userId));
            }
            else
            {
                return Ok(new UserModel());
            }
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UserModel> Post([FromBody] UserModel userModel)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            userModel.TimeCreated = DateTime.Now;
            userModel.TimeLastEdit = DateTime.Now;
            userModel = _usersDbContext.Post(userModel);
            
            return Ok(userModel);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public ActionResult<UserModel> Put(string id, [FromBody] UpdateUserModel userModel)
        {
            UserModel result;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            var user = new UserModel { 
                Password = userModel.OldPassword, 
                Username = userModel.Username,
                Email = userModel.Email,
                Id = userModel.Id,
                TimeLastEdit = DateTime.Now
            };

            if (_usersDbContext.Get(user) != null)
            {
                user.Password = UserService.HashPassword(userModel.Username, userModel.NewPassword);
                result = _usersDbContext.Put(id, user);
            } 
            else
            {
                result = user;
            }

            return Ok(result);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _usersDbContext.Delete(id);

            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize]
        public ActionResult<Response> Authenticated()
        {
            // If bearer token is correct return 200 OK.
            return Ok(new Response() { Status = "Success", Message = "Authenticated" });
        }
        
        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult<Response> Authenticate([FromBody] UserModel user)
        {
            try
            {
                // Authenticates a user using username and password.
                user = _userService.Authenticate(user);

                // If user exists and credentials are correct
                if (user != null)
                {
                    // Return a cookie in the response.
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
                    return Ok(new Response { Status = "Failure", Message = "Invalid Username or Password" });
                }
            }
            catch
            {
                return Ok(new Response { Status = "Error", Message = "Something went wrong, please try again." });
            }
        }
    }
}
