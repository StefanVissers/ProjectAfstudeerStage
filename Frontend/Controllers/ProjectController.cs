using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SSLLabsApiWrapper;
using static SSLLabsApiWrapper.SSLLabsApiService;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectDbContext _projectsDbContext;
        private readonly MongoDBAppSettings _mongoDbSettings;

        public ProjectController(IOptions<MongoDBAppSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _projectsDbContext = new ProjectDbContext(_mongoDbSettings);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        }

        // GET: api/Project/SSLLabs/5da829fa67db7d33e88a5d9e/http://google.nl/192.168.1.1
        [HttpPost("SSLLabs/{id}")]
        public IActionResult GetSSLLabsReport(string id, [FromBody]SSLLabsRequestModel requestModel)
        {
            var ssllService = new SSLLabsApiService("https://api.ssllabs.com/api/v3");
            var x = ssllService.Analyze(host: requestModel.Host, publish: Publish.Off, startNew: StartNew.Ignore,
                fromCache: FromCache.Off, maxHours: 1, all: All.On, ignoreMismatch: IgnoreMismatch.Off);

            var z = ssllService.GetEndpointData(host: requestModel.Host, s: requestModel.Ip);
            while (z.statusMessage == "In progress" || z.statusMessage == "Pending" || x.status == "IN_PROGRESS")
            {
                Thread.Sleep(5000);
                z = ssllService.GetEndpointData(requestModel.Host, requestModel.Ip);
                
                x = ssllService.Analyze(host: requestModel.Host, publish: Publish.Off, startNew: StartNew.Ignore,
                fromCache: FromCache.Off, maxHours: 1, all: All.On, ignoreMismatch: IgnoreMismatch.Off);
            }

            if (x.Errors.Any())
            {
                if (x.Errors.First().message.Contains("529"))
                {
                    return Ok(new { Status = "Service Overloaded", Body = "" });
                } 
                else if (x.Errors.First().message.Contains("503"))
                {
                    return Ok(new { Status = "Service Unavailable", Body = "" });
                }
                else if (x.Errors.First().message.Contains("500"))
                {
                    return Ok(new { Status = "Service Error", Body = "" });
                }
                else if (x.Errors.First().message.Contains("429"))
                {
                    return Ok(new { Status = "Too Many Requests", Body = "" });
                }
                else if (x.Errors.First().message.Contains("400"))
                {
                    return Ok(new { Status = "Invalid Parameters", Body = "" });
                }
            }

            // Return the raw response because the model in the wrapper is not up to date.
            return Ok(x.Wrapper.ApiRawResponse);
        }

        // GET: api/Project
        [HttpGet("")]
        public IActionResult Get()
        {
            var result = _projectsDbContext.Get();

            // Get the UserId from the Claims.
            var user = User.Identity as ClaimsIdentity;
            var userId = user.FindFirst(ClaimTypes.Name)?.Value;

            if (userId != null)
            {
                result = result.Where(x => x.Users.Any(y => y.UserId == userId));
            }

            result = result.OrderByDescending(x => x.IsCompleted ? 0 : 1);

            return Ok(result);
        }

        // GET: api/Project/5da829fa67db7d33e88a5d9e
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _projectsDbContext.Get(id);

            return Ok(result);
        }

        [HttpGet("{id}/{categoryid}")]
        public IActionResult Get(string id, string categoryid)
        {
            var project = _projectsDbContext.Get(id);
            var category = project.WorkflowElementCategories.Where(x => x.CategoryId == categoryid).FirstOrDefault();

            return Ok(category);
        }

        [HttpGet("{id}/{categoryid}/{elementid}")]
        public IActionResult Get(string id, string categoryid, string elementid)
        {
            var project = _projectsDbContext.Get(id);
            var category = project.WorkflowElementCategories.Where(x => x.CategoryId == categoryid).FirstOrDefault();
            var element = category.WorkflowElements.Where(x => x.ElementId == elementid).FirstOrDefault();
            return Ok(element);
        }

        [HttpGet("Users/{id}")]
        public IActionResult Users(string id)
        {
            var project = _projectsDbContext.Get(id);

            return Ok(project.Users);
        }

        [HttpGet("UserRoles")]
        public IActionResult UserRoles()
        {
            var roles = UserRole.UserRoles;

            return Ok(roles);
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult Post([FromBody] ProjectModel project)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            project.TimeCreated = DateTime.Now;
            project.TimeLastEdit = DateTime.Now;

            if (project.Users == null)
            {
                project.Users = new List<UserRole>();
            }

            // Get the UserId from the Claims.
            var user = User.Identity as ClaimsIdentity;
            var userId = user.FindFirst(ClaimTypes.Name)?.Value;

            if (userId != null)
            {
                var userDbContext = new UsersDbContext(_mongoDbSettings);
                var userModel = userDbContext.Get(userId);

                var userRole = new UserRole() { UserId = userId, Name = userModel.Username, Role = UserRole.UserRoleCreator };

                project.Users.Add(userRole);
            }

            var result = _projectsDbContext.Post(project);
            
            return Ok(result);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] ProjectModel project)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            project.TimeLastEdit = DateTime.Now;
            
            var result = _projectsDbContext.Put(id, project);

            return Ok(result);
        }

        // PUT: api/Project/5
        [HttpPut("Users/{id}")]
        public IActionResult Put(string id, [FromBody] IEnumerable<UserRole> value)
        {
            var result = _projectsDbContext.Put(id, value);

            return Ok(result);
        }

        [HttpPut("{projectId}/{categoryId}")]
        public IActionResult Put(string projectId, string categoryId, [FromBody] WorkflowElement value)
        {
            var result = _projectsDbContext.Put(projectId, categoryId, value);

            return Ok(result);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _projectsDbContext.Delete(id);

            return Ok();
        }


        [HttpGet("Authenticated/{id}")]
        [Authorize]
        public IActionResult Authenticated(string id)
        {
            var user = User.Identity as ClaimsIdentity;

            var userId = user.FindFirst(ClaimTypes.Name)?.Value;

            var project = _projectsDbContext.Get(id);

            if(project.Users.Any(x => x.UserId == userId))
             {
                return Ok("Succes");
            }

            return Ok("Failure");
        }
    }
}
