﻿using MongoDB.Driver;
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

        public IEnumerable<ProjectModel> Get()
        {
            List<ProjectModel> list = ProjectsCollection.Find(EmptyProjectFilter).ToList();

            return list;
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
            var template = ProjectsCollection.Find(TemplateProjectFilter).FirstOrDefault();

            model.WorkflowElementCategories = template.WorkflowElementCategories;

            ProjectsCollection.InsertOne(model);

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
        IEnumerable<ProjectModel> Get();
        IMongoCollection<ProjectModel> ProjectsCollection { get; }
    }
}