using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Frontend.Models;
using Frontend.Services;
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
        private readonly IUsersDbContext _usersDbContext;
        private readonly IToolingService _toolingService;

        public ProjectController(IUsersDbContext usersDbContext, IProjectDbContext dbContext, IToolingService toolingService)
        {
            _usersDbContext = usersDbContext;
            _projectsDbContext = dbContext;
            _toolingService = toolingService;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        }

        // GET: api/Project/SSLLabs/5da829fa67db7d33e88a5d9e/
        [HttpGet("[action]/{id}")]
        public ActionResult<string> GetSSLLabsReport(string id)
        {
            var project = _projectsDbContext.Get(id);

            return Ok(project.SSLLabsData);
        }

        // POST: api/Project/SSLLabs/5da829fa67db7d33e88a5d9e/
        [HttpPost("SSLLabs/{id}")]
        public IActionResult GetSSLLabsReport(string id, [FromBody]SSLLabsRequestModel requestModel)
        {
            var project = _projectsDbContext.Get(id);

            // check for null values
            if (requestModel == null || requestModel.Host == null)
            {
                return Ok(new { Status = "Please enter a hostname and ip address", Body = "" });
            }
            
            // Start the scan or get a already started scan.
            var ssllService = new SSLLabsApiService("https://api.ssllabs.com/api/v3");
            var analyze = ssllService.Analyze(host: requestModel.Host, publish: Publish.Off, startNew: StartNew.Ignore,
                fromCache: FromCache.Off, maxHours: 1, all: All.On, ignoreMismatch: IgnoreMismatch.Off);

            // Check every 8 seconds to see if the scan has completed.
            while (analyze.status == "IN_PROGRESS" || analyze.status != "READY")
            {
                Thread.Sleep(8000);

                analyze = ssllService.Analyze(host: requestModel.Host, publish: Publish.Off, startNew: StartNew.Ignore,
                fromCache: FromCache.Off, maxHours: 1, all: All.On, ignoreMismatch: IgnoreMismatch.Off);
            }

            // If there are errors, return error status to notify the user.
            if (analyze.Errors.Any())
            {
                if (analyze.Errors.First().message.Contains("529"))
                {
                    return Ok(new { Status = "Service Overloaded", Body = "" });
                }
                else if (analyze.Errors.First().message.Contains("503"))
                {
                    return Ok(new { Status = "Service Unavailable", Body = "" });
                }
                else if (analyze.Errors.First().message.Contains("500"))
                {
                    return Ok(new { Status = "Service Error", Body = "" });
                }
                else if (analyze.Errors.First().message.Contains("429"))
                {
                    return Ok(new { Status = "Too Many Requests", Body = "" });
                }
                else if (analyze.Errors.First().message.Contains("400"))
                {
                    return Ok(new { Status = "Invalid Parameters", Body = "" });
                }
            }

            // Save the api responsedata.
            project.SSLLabsDataTimeLastScan = DateTime.Now;
            project.SSLLabsData = analyze.Wrapper.ApiRawResponse;
            _projectsDbContext.Put(id, project);

            // Return the raw response because the model in the wrapper is not up to date.
            return Ok(analyze.Wrapper.ApiRawResponse);
        }

        [HttpPost("[action]/{id}")]
        public ActionResult KaliLinuxTool(string id, [FromBody] Command command)
        {
            CommandResult result;
            var project = _projectsDbContext.Get(id);
            try
            {
                result = _toolingService.Execute(command);
            }
            catch (Exception objException)
            {
                return Ok(objException);
            }
            project.CommandResult = result;
            _projectsDbContext.Put(id, project);

            return Ok(result);
        }

        // GET: api/Project
        [HttpGet("")]
        public ActionResult<IEnumerable<ProjectModel>> Get()
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
        public ActionResult<ProjectModel> Get(string id)
        {
            try
            {
                var result = _projectsDbContext.Get(id);

                return Ok(result);
            }
            catch
            {
                return Ok(null);
            }
        }

        [HttpGet("{id}/{categoryid}")]
        public ActionResult<WorkflowElementCategory> Get(string id, string categoryid)
        {
            var project = _projectsDbContext.Get(id);
            var category = project.WorkflowElementCategories.Where(x => x.CategoryId == categoryid).FirstOrDefault();

            return Ok(category);
        }

        [HttpGet("{id}/{categoryid}/{elementid}")]
        public ActionResult<WorkflowElement> Get(string id, string categoryid, string elementid)
        {
            var project = _projectsDbContext.Get(id);
            var category = project.WorkflowElementCategories.Where(x => x.CategoryId == categoryid).FirstOrDefault();
            var element = category.WorkflowElements.Where(x => x.ElementId == elementid).FirstOrDefault();
            return Ok(element);
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<UserModel> Users(string id)
        {
            var project = _projectsDbContext.Get(id);

            return Ok(project.Users);
        }

        [HttpGet("[action]")]
        public ActionResult<string[]> UserRoles()
        {
            var roles = UserRole.UserRoles;

            return Ok(roles);
        }

        // POST: api/Project
        [HttpPost]
        public ActionResult<ProjectModel> Post([FromBody] ProjectModel project)
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
                var userModel = _usersDbContext.Get(userId);

                var userRole = new UserRole() { UserId = userId, Name = userModel.Username, Role = UserRole.UserRoleCreator };

                project.Users.Add(userRole);
            }

            var result = _projectsDbContext.Post(project);
            
            return Ok(result);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public ActionResult<ProjectModel> Put(string id, [FromBody] ProjectModel project)
        {   
            var result = _projectsDbContext.Put(id, project);

            return Ok(result);
        }

        // PUT: api/Project/5
        [HttpPut("[action]/{id}")]
        public ActionResult<ProjectModel> Put(string id, [FromBody] IEnumerable<UserRole> value)
        {
            var result = _projectsDbContext.Put(id, value);

            return Ok(result);
        }

        [HttpPut("{projectId}/{categoryId}")]
        public ActionResult<ProjectModel> Put(string projectId, string categoryId, [FromBody] WorkflowElement value)
        {
            var result = _projectsDbContext.Put(projectId, categoryId, value);

            return Ok(result);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _projectsDbContext.Delete(id);

            return Ok();
        }


        [HttpGet("[action]/{id}")]
        [Authorize]
        public ActionResult<string> Authenticated(string id)
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
