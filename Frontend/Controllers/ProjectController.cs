using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectDbContext _projectsDbContext;
        private readonly MongoDBAppSettings _mongoDbSettings;

        public ProjectController(IOptions<MongoDBAppSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _projectsDbContext = new ProjectDbContext(_mongoDbSettings);
        }



        // GET: api/Project
        [HttpGet]
        public IActionResult Get()
        {
            var result = _projectsDbContext.Get();

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
        public IActionResult Put(string id, [FromBody] ProjectModel value)
        {
            var result = _projectsDbContext.Put(id, value);

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

            return Ok();
        }
    }
}
