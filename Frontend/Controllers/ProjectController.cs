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
