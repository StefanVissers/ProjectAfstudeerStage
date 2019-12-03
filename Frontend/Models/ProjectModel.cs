using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SSLLabsApiWrapper.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public List<UserRole> Users { get; set; }

        [BsonElement()]
        public List<WorkflowElementCategory> WorkflowElementCategories { get; set; }

        [BsonElement()]
        public List<Endpoint> SSLLabsData { get; set; }

        // Meta Data
        [BsonElement()]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TimeCreated { get; set; }

        [BsonElement()]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TimeLastEdit { get; set; }

    }

    public class WorkflowElementCategory
    {
        [BsonElement()]
        public string CategoryId { get; set; }

        [BsonElement()]
        public string Name { get; set; }

        [BsonElement()]
        public string Description { get; set; }

        [BsonElement()]
        public List<WorkflowElement> WorkflowElements { get; set; }
    }

    public class WorkflowElement
    {
        [BsonElement()]
        public string ElementId { get; set; }

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

    public class UserRole
    {
        public const string UserRoleCreator = "Creator";
        public const string UserRoleExtra = "Extra";
        public const string UserRoleViewer = "Viewer";
        public static readonly string[] UserRoles = { UserRoleCreator, UserRoleExtra, UserRoleViewer };

        [BsonElement()]
        public string UserId { get; set; }

        [BsonElement()]
        public string Name { get; set; }

        [BsonElement()]
        public string Role { get; set; }
    }

    public class ProjectListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public int ASVSLevel { get; set; }
    }

    public class SSLLabsRequestModel
    {
        public string Host { get; set; }

        public string Ip { get; set; }
    }
}
