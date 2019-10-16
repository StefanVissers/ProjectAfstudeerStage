using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class ProjectModel
    {
        // Main Stuff

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement()]
        public string Name { get; set; }

        [BsonElement()]
        public string Description { get; set; }

        [BsonElement()]
        public bool IsCompleted { get; set; }

        // Data
        [BsonElement()]
        public int ASVSLevel { get; set; }

        [BsonElement()]
        public WorkflowElementCategory[] WorkflowElementCategories { get; set; }
    }

    public class WorkflowElementCategory
    {
        [BsonElement()]
        public string Name { get; set; }

        [BsonElement()]
        public string Description { get; set; }

        [BsonElement()]
        public WorkflowElement[] WorkflowElements { get; set; }
    }

    public class WorkflowElement
    {
        [BsonElement()]
        public string Name { get; set; }

        [BsonElement()]
        public string Description { get; set; }
        
        [BsonElement()]
        public string Explanation { get; set; }

        [BsonElement()]
        public bool IsDone { get; set; }

        [BsonElement()]
        public bool IsRelevant { get; set; }
    }

    public enum ASVSLevel
    {
        LevelOne = 1,
        LevelTwo = 2,
        LevelThree = 3
    }
}
