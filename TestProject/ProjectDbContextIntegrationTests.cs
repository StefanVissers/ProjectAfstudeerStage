using Frontend.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class ProjectDbContextIntegrationTests
    {
        private readonly ProjectDbContext projectDbContext;

        public ProjectDbContextIntegrationTests()
        {
            // ALWAYS USE A TEST DATABASE. NOT PRODUCTION.
            var mongoDbOptions = Options.Create(new MongoDBAppSettings()
            {
                MongoDatabaseName = "TESTProjects",
                MongoUserDatabaseName = "TESTUsers",
                MongoAuthDatabaseName = "TESTadmin",
                MongoUsername = "TESTAdmin",
                MongoPassword = "TESTsecure123",
                MongoPort = "27017",
                MongoHost = "localhost"
            });

            projectDbContext = new ProjectDbContext(mongoDbOptions);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var projects = projectDbContext.Get() as List<ProjectModel>;
            var templateProject = projects.Find(x => x.Name == "Template");
            Assert.IsNotNull(projects);

            // The Template Project should always be in the database.
            Assert.AreEqual(1, projects.Count);

            Assert.IsNotNull(templateProject);
            Assert.AreEqual("Template Project", templateProject.Description);
        }

        // Insert and Delete because database should be clean after method.
        [TestMethod]
        public void InsertAndDeleteOneTest()
        {
            // These are static small projects used for testing.
            var projects = MockClasses.MockProjectDbContext.projects;
            
            foreach(var project in projects)
            {
                projectDbContext.Post(project);
            }

            var projectsDb = projectDbContext.Get() as List<ProjectModel>;

            Assert.AreEqual(4, projectsDb.Count);

            foreach(var project in projects)
            {
                projectDbContext.Delete(project.Id);
            }

            projectsDb = projectDbContext.Get() as List<ProjectModel>;

            Assert.AreEqual(1, projectsDb.Count);
        }
    }
}
