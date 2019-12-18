using Frontend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject.MockClasses
{
    class MockProjectDbContext : IProjectDbContext
    {
        public MockProjectDbContext()
        {
        }

        // Make some mock data.
        private static List<WorkflowElementCategory> elementCategories = new List<WorkflowElementCategory>()
        {
            new WorkflowElementCategory()
            {
                Name = "Category0",
                Description = "Cat0Descr",
                CategoryId = "0",
                WorkflowElements = new List<WorkflowElement>()
                {
                    new WorkflowElement()
                    {
                        ElementId = "0",
                        Description = "Element0Descr",
                        Name = "Element0",
                        Explanation = "Element0Expl",
                        IsDone = false,
                        IsRelevant = true
                    },
                    new WorkflowElement()
                    {
                        ElementId = "1",
                        Description = "Element1Descr",
                        Name = "Element1",
                        Explanation = "Element1Expl",
                        IsDone = false,
                        IsRelevant = false
                    },
                    new WorkflowElement()
                    {
                        ElementId = "2",
                        Description = "Element2Descr",
                        Name = "Element2",
                        Explanation = "Element2Expl",
                        IsDone = true,
                        IsRelevant = true
                    }
                }
            },
            new WorkflowElementCategory()
            {
                Name = "Category1",
                Description = "Cat1Descr",
                CategoryId = "1",
                WorkflowElements = new List<WorkflowElement>()
                {
                    new WorkflowElement()
                    {
                        ElementId = "0",
                        Description = "Element0Descr",
                        Name = "Element0",
                        Explanation = "Element0Expl",
                        IsDone = false,
                        IsRelevant = true
                    },
                    new WorkflowElement()
                    {
                        ElementId = "1",
                        Description = "Element1Descr",
                        Name = "Element1",
                        Explanation = "Element1Expl",
                        IsDone = false,
                        IsRelevant = false
                    },
                    new WorkflowElement()
                    {
                        ElementId = "2",
                        Description = "Element2Descr",
                        Name = "Element2",
                        Explanation = "Element2Expl",
                        IsDone = true,
                        IsRelevant = true
                    }
                }
            }
        };

        // More mock data.
        private static List<UserRole> users = new List<UserRole>()
        {
            new UserRole()
            {
                UserId = "5cccc9fa67db7d35e88a5d9f",
                Name = "Mock sir Mocking",
                Role = "0"
            },
            new UserRole()
            {
                UserId = "ddddd9fa67dddd35e88a5d9f",
                Name = "Ms Mocking Mock",
                Role = "1"
            }
        };

        // Even more mock data.
        private List<ProjectModel> projects = new List<ProjectModel>()
        {
            new ProjectModel()
            {
                Id = "5da829fa67db7d33e88a5d9e",
                ASVSLevel = 2,
                Description = "This is a test project",
                Name = "test1",
                IsCompleted = false,
                SSLLabsData = null,
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                WorkflowElementCategories = elementCategories,
                Users = users
            },
            new ProjectModel()
            {
                Id = "bbb829fa67db7d33e88a5d9f",
                ASVSLevel = 3,
                Description = "This is the second test project",
                Name = "test2",
                IsCompleted = false,
                SSLLabsData = null,
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                WorkflowElementCategories = elementCategories,
                Users = users
            },
            new ProjectModel()
            {
                Id = "aaa829fa67db7d33e88a5d9a",
                ASVSLevel = 1,
                Description = "This is the third test project",
                Name = "test3",
                IsCompleted = false,
                SSLLabsData = null,
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                WorkflowElementCategories = elementCategories,
                Users = new List<UserRole>(){ users[1] }
            }
        };

        // Not used in the mock object.
        public IMongoCollection<ProjectModel> ProjectsCollection => throw new NotImplementedException();

        public void Delete(string id)
        {
            var proj = projects.Find(x => x.Id == id);
            projects.Remove(proj);
        }

        public IEnumerable<ProjectModel> Get()
        {
            return projects;
        }

        public ProjectModel Get(string id)
        {
            return projects.Find(x => x.Id == id);
        }

        public ProjectModel Post(ProjectModel model)
        {
            projects.Add(model);

            return model;
        }

        public ProjectModel Put(string projectId, IEnumerable<UserRole> value)
        {
            var users = value as List<UserRole>;
            projects.Find(x => x.Id == projectId).Users = users;
            return projects.Find(x => x.Id == projectId);
        }

        public ProjectModel Put(string projectId, string categoryId, WorkflowElement workflowElement)
        {
            var project = Get(projectId);
            var category = project.WorkflowElementCategories.First(x => x.CategoryId == categoryId);
            var element = category.WorkflowElements.First(x => x.ElementId == workflowElement.ElementId);

            element.Explanation = workflowElement.Explanation;
            element.IsDone = workflowElement.IsDone;
            element.IsRelevant = workflowElement.IsRelevant;
            element.Description = workflowElement.Description;

            var newProject = ProjectsCollection.ReplaceOne(x => x.Id == projectId, project);

            return project;
        }

        public ProjectModel Put(string id, ProjectModel model)
        {
            int index = projects.FindIndex(x => x.Id == id);
            projects[index] = model;

            return model;
        }
    }
}
