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

        public ProjectModel Get(ProjectModel projectModel)
        {
            var filterName = Builders<ProjectModel>.Filter.Eq(x => x.Name, projectModel.Name);
            var filterDescription = Builders<ProjectModel>.Filter.Eq(x => x.Description, projectModel.Description);
            var filterCombined = Builders<ProjectModel>.Filter.And(filterName, filterDescription);

            var project = ProjectsCollection.FindAsync(filterCombined).Result.FirstOrDefault();

            return project;
        }

        public ProjectModel Get(string id)
        {
            var filterId = Builders<ProjectModel>.Filter.Eq(x => x.Id, id);
            var project = ProjectsCollection.FindAsync(filterId).Result.FirstOrDefault();

            return project;
        }

        public ProjectModel Post(ProjectModel model)
        {
            if (Get(model) == null)
            {
                model.WorkflowElementCategories = new WorkflowElementCategory[2];
                model.WorkflowElementCategories[0] = new WorkflowElementCategory()
                {
                    Description = "TestDescription",
                    Name = "Test1Category",
                    WorkflowElements = new WorkflowElement[2]
                };
                model.WorkflowElementCategories[1] = new WorkflowElementCategory()
                {
                    Name = "Test2Category",
                    Description = "test2description",
                    WorkflowElements = new WorkflowElement[1]
                };
                model.WorkflowElementCategories[0].WorkflowElements[0] = new WorkflowElement()
                {
                    Description = "worflowelementdescription1",
                    Name = "worflowelementname1",
                    Explanation = "explanation 1",
                    IsDone = true,
                    IsRelevant = true
                };
                model.WorkflowElementCategories[0].WorkflowElements[1] = new WorkflowElement()
                {
                    Description = "worflowelementdescription2",
                    Name = "worflowelementname2",
                    Explanation = "explanation 2",
                    IsDone = false,
                    IsRelevant = true
                };
                model.WorkflowElementCategories[1].WorkflowElements[0] = new WorkflowElement()
                {
                    Description = "worflowelementdescription2.1",
                    Name = "worflowelementname2.1",
                    Explanation = "explanation 2.1",
                    IsDone = true,
                    IsRelevant = false
                };
                ProjectsCollection.InsertOne(model);
            }
            return model;
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
        IMongoCollection<ProjectModel> ProjectsCollection { get; }
    }
}
