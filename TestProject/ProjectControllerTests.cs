using Frontend.Controllers;
using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using TestProject.MockClasses;

namespace TestProject
{
    [TestClass]
    public class ProjectControllerTests
    {
        private ProjectController _projectController;

        public ProjectControllerTests()
        {
            // Create a ClaimsPrincipal to mock a logged in user.
            var identity = new GenericIdentity("5cccc9fa67db7d35e88a5d9f");
            var claimsIdentity = new ClaimsIdentity(identity);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            _projectController = new ProjectController(new MockUserDbContext(), new MockProjectDbContext());
            _projectController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };
        }

        [TestMethod]
        public void GetAllTest()
        {
            var response = _projectController.Get();
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            var projects = okObjectResult.Value as IOrderedEnumerable<ProjectModel>;
            Assert.IsNotNull(projects);
            Assert.AreEqual(projects.Count(), 3);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("5da829fa67db7d33e88a5d9e")]
        [DataRow("bbb829fa67db7d33e88a5d9f")]
        [DataRow("aaa829fa67db7d33e88a5d9a")]
        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaa")]
        public void GetSpecificTest(string id)
        {
            var response = _projectController.Get(id);
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            var project = okObjectResult.Value as ProjectModel;
            if (id != "")
            {
                Assert.IsNotNull(project);
                Assert.IsNotNull(project.WorkflowElementCategories);
                Assert.IsNotNull(project.Users);
                Assert.IsNotNull(project.Name);
                Assert.AreEqual(project.Id, id);
            }
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("5da829fa67db7d33e88a5d9e")]
        [DataRow("bbb829fa67db7d33e88a5d9f")]
        [DataRow("aaa829fa67db7d33e88a5d9a")]
        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaa")]
        public void DeleteTest(string id)
        {
            var response = _projectController.Get();
            var okObjectResult = response.Result as OkObjectResult;
            var projects = okObjectResult.Value as IOrderedEnumerable<ProjectModel>;
            Assert.AreEqual(projects.Count(), 3);

            response = _projectController.Delete(id);
            var okResult = response.Result as OkResult;
            Assert.AreEqual(okResult.StatusCode, 200);

            if (id != "" && id != "aaaaaaaaaaaaaaaaaaaaaaaa")
            {
                response = _projectController.Get();
                okObjectResult = response.Result as OkObjectResult;
                projects = okObjectResult.Value as IOrderedEnumerable<ProjectModel>;
                Assert.AreEqual(projects.Count(), 2);
            }
            else
            {
                response = _projectController.Get();
                okObjectResult = response.Result as OkObjectResult;
                projects = okObjectResult.Value as IOrderedEnumerable<ProjectModel>;
                Assert.AreEqual(projects.Count(), 3);
            }
        }
    }
}
