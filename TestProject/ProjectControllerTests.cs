using Frontend.Controllers;
using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            _projectController = new ProjectController(
                    new MockUserDbContext(),
                    new MockProjectDbContext(),
                    new MockToolingService());
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
        public void GetProjectTest(string id)
        {
            var response = _projectController.Get(id);
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var project = okObjectResult.Value as ProjectModel;
            if (id != "" && id != "aaaaaaaaaaaaaaaaaaaaaaaa")
            {
                Assert.IsNotNull(project);
                Assert.IsNotNull(project.WorkflowElementCategories);
                Assert.IsNotNull(project.Users);
                Assert.IsNotNull(project.Name);
                Assert.AreEqual(id, project.Id);
            }
            else
            {
                Assert.IsNull(project);
            }
        }

        [DataTestMethod]
        [DataRow("5da829fa67db7d33e88a5d9e", "0")]
        [DataRow("5da829fa67db7d33e88a5d9e", "1")]
        [DataRow("bbb829fa67db7d33e88a5d9f", "1")]
        [DataRow("aaa829fa67db7d33e88a5d9a", "0")]
        public void GetCategoryTest(string projectId, string categoryId)
        {
            var response = _projectController.Get(projectId, categoryId);
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var category = okObjectResult.Value as WorkflowElementCategory;
            Assert.AreEqual(categoryId, category.CategoryId);
        }

        [DataTestMethod]
        [DataRow("5da829fa67db7d33e88a5d9e", "0", "0")]
        [DataRow("5da829fa67db7d33e88a5d9e", "0", "1")]
        [DataRow("5da829fa67db7d33e88a5d9e", "1", "1")]
        [DataRow("5da829fa67db7d33e88a5d9e", "1", "0")]
        [DataRow("bbb829fa67db7d33e88a5d9f", "1", "0")]
        [DataRow("aaa829fa67db7d33e88a5d9a", "0", "0")]
        public void GetCategoryTest(string projectId, string categoryId, string elementId)
        {
            var response = _projectController.Get(projectId, categoryId, elementId);
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var element = okObjectResult.Value as WorkflowElement;
            Assert.AreEqual(elementId, element.ElementId);
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

        [DataTestMethod]
        [DataRow("5da829fa67db7d33e88a5d9e")]
        [DataRow("bbb829fa67db7d33e88a5d9f")]
        [DataRow("aaa829fa67db7d33e88a5d9a")]
        public void PutTestProject(string id)
        {
            var response = _projectController.Get(id);
            var okObjectResult = response.Result as OkObjectResult;
            var project = okObjectResult.Value as ProjectModel;
            project.Name = "Changed Name";
            project.Description = "Changed Description";
            project.IsCompleted = true;

            response = _projectController.Put(id, project);
            Assert.IsNotNull(response);
            okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            project = okObjectResult.Value as ProjectModel;
            Assert.IsNotNull(project);
        }

        [DataTestMethod]
        [DataRow("5da829fa67db7d33e88a5d9e")]
        [DataRow("bbb829fa67db7d33e88a5d9f")]
        [DataRow("aaa829fa67db7d33e88a5d9a")]
        public void PutTestElement(string id)
        {
            var response = _projectController.Get(id);
            var okObjectResult = response.Result as OkObjectResult;
            var project = okObjectResult.Value as ProjectModel;

            var users = project.Users;
            users.Add(new UserRole
            {
                UserId = "eeeed9fa67dddd35e88a5d9f",
                Name = "Little Mocking Mock",
                Role = "1"
            });

            response = _projectController.Put(id, users);
            okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            project = okObjectResult.Value as ProjectModel;
            Assert.IsNotNull(project);
        }
    }
}
