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
    public class ProjectController : ControllerBase
    {
        private readonly IDbContext<ProjectModel> _projectsDbContext;
        private readonly MongoDBAppSettings _mongoDbSettings;

        public ProjectController(IOptions<MongoDBAppSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _projectsDbContext = new ProjectDbContext(_mongoDbSettings);
        }



        // GET: api/Project
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult Post([FromBody] ProjectModel project)
        {
            var result = _projectsDbContext.Post(project);

            return Ok(result);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
