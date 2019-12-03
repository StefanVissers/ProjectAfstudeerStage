using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class ProjectDbContext : IProjectDbContext
    {
        private readonly FilterDefinition<ProjectModel> EmptyProjectFilter = Builders<ProjectModel>.Filter.Empty;
        private readonly FilterDefinition<ProjectModel> TemplateProjectFilter = Builders<ProjectModel>.Filter
            .And(Builders<ProjectModel>.Filter.Eq(x => x.Name, "Template"),
            Builders<ProjectModel>.Filter.Eq(x => x.Description, "Template Project"));

        MongoClient _client;
        public IMongoDatabase _database;
        private readonly MongoDBAppSettings Config;

        public ProjectDbContext(MongoDBAppSettings config)
        {
            // Uses the config.
            Config = config;

            // Gets values out of the config.
            var MongoDatabaseName = Config.MongoUserDatabaseName;
            var MongoAuthDatabaseName = Config.MongoAuthDatabaseName;
            var MongoUsername = Config.MongoUsername;
            var MongoPassword = Config.MongoPassword;
            var MongoPort = Config.MongoPort;
            var MongoHost = Config.MongoHost;

            // Creates a model to log into the mongodb.
            //MongoCredential credential = MongoCredential.CreateCredential
            //(MongoAuthDatabaseName,
            // MongoUsername,
            // MongoPassword);

            // Connects to the Db.
            var settings = new MongoClientSettings
            {
                //Credential = credential,
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);

            // Gets the corrext db.
            _database = _client.GetDatabase("Projects");
        }

        /// <summary>
        /// Gets a list of all projects without workflowelementcategories.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectModel> Get()
        {
            var list = ProjectsCollection.Find(EmptyProjectFilter).ToList();

            // Parallel so requests will be faster.
            Parallel.ForEach(list, project => {
                project.WorkflowElementCategories = null;
            });

            return list;
        }

        /// <summary>
        /// Gets a ProjectModel from its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectModel Get(string id)
        {
            var filterId = Builders<ProjectModel>.Filter.Eq(x => x.Id, id);
            var project = ProjectsCollection.FindAsync(filterId).Result.FirstOrDefault();

            return project;
        }

        public ProjectModel Post(ProjectModel model)
        {
            var template = ProjectsCollection.Find(TemplateProjectFilter).FirstOrDefault();

            model.WorkflowElementCategories = template.WorkflowElementCategories;

            ProjectsCollection.InsertOne(model);

            return model;
        }


        public ProjectModel Put(string id, ProjectModel model)
        {
            var filterId = Builders<ProjectModel>.Filter.Eq(x => x.Id, id);

            var updateProjectName = Builders<ProjectModel>.Update.Set(x => x.Name, model.Name);
            var updateProjectDescription = Builders<ProjectModel>.Update.Set(x => x.Description, model.Description);
            var updateProjectASVSLevel = Builders<ProjectModel>.Update.Set(x => x.ASVSLevel, model.ASVSLevel);
            var updateProjectIsCompleted = Builders<ProjectModel>.Update.Set(x => x.IsCompleted, model.IsCompleted);
            var updateProjectUsers = Builders<ProjectModel>.Update.Set(x => x.Users, model.Users);
            var updateProjectElements = Builders<ProjectModel>.Update.Set(x => x.WorkflowElementCategories, model.WorkflowElementCategories);
            var updateProjectTimeLastEdit = Builders<ProjectModel>.Update.Set(x => x.TimeLastEdit, model.TimeLastEdit);

            var updates = Builders<ProjectModel>.Update.Combine(
                updateProjectName, updateProjectDescription, updateProjectASVSLevel,
                updateProjectIsCompleted, updateProjectUsers, updateProjectElements,
                updateProjectTimeLastEdit);

            var project = ProjectsCollection.FindOneAndUpdate(filterId, updates);

            return project;
        }

        /// <summary>
        /// Updates the Users list of a project.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ProjectModel Put(string id, IEnumerable<UserRole> model)
        {
            var filterId = Builders<ProjectModel>.Filter.Eq(x => x.Id, id);

            var updateProjectUsers = Builders<ProjectModel>.Update.Set(x => x.Users, model);


            var project = ProjectsCollection.FindOneAndUpdate(filterId, updateProjectUsers);

            return project;
        }

        /// <summary>
        /// Updates a workflowelement in a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="categoryId"></param>
        /// <param name="workflowElement"></param>
        /// <returns></returns>
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

        public void Delete(string id)
        {
            var filterId = Builders<ProjectModel>.Filter.Eq(x => x.Id, id);

            ProjectsCollection.FindOneAndDelete(filterId);
        }

        public IMongoCollection<ProjectModel> ProjectsCollection
        {
            get
            {
                return _database.GetCollection<ProjectModel>("ProjectRecord");
            }
        }
    }

    interface IProjectDbContext : IDbContext<ProjectModel>
    {
        IEnumerable<ProjectModel> Get();
        ProjectModel Put(string projectId, IEnumerable<UserRole> value);
        ProjectModel Put(string projectId, string categoryId, WorkflowElement workflowElement);
        IMongoCollection<ProjectModel> ProjectsCollection { get; }
    }
}
