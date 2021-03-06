﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression("[123]")]
        public int ASVSLevel { get; set; }

        [BsonElement()]
        public List<UserRole> Users { get; set; }

        [BsonElement()]
        public List<WorkflowElementCategory> WorkflowElementCategories { get; set; }

        // We save the raw response
        [BsonElement()]
        public string SSLLabsData { get; set; }

        [BsonElement()]
        public CommandResult CommandResult { get; set; }

        [BsonElement()]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime SSLLabsDataTimeLastScan { get; set; }

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

        [BsonIgnore]
        public bool IsCompleted { get
            {
                return WorkflowElements.TrueForAll(x => x.IsCompleted);
            } }
    }

    public class WorkflowElement
    {
        [BsonElement()]
        public string ElementId { get; set; }

        [BsonElement()]
        public int ASVSLevel { get; set; }

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

        [BsonIgnore]
        public bool IsCompleted { get
            {
                return (IsDone || !IsRelevant);
            } }
    }

    public class Command
    {
        public string NmapAction { get; set; }
        
        public string NiktoAction { get; set; }

        public string XsserAction { get; set; }

        [RegularExpression("[^|&$;`]+", ErrorMessage = "Blacklisted characters used.")]
        public string NmapAdditionalArgs { get; set; }

        [RegularExpression("[^|&$;`]+", ErrorMessage = "Blacklisted characters used.")]
        public string NiktoAdditionalArgs { get; set; }

        [RegularExpression("[^|&$;`]+", ErrorMessage = "Blacklisted characters used.")]
        public string XsserAdditionalArgs { get; set; }

        [RegularExpression("[^|&$;`]+", ErrorMessage = "Blacklisted characters used.")]
        public string Hostname { get; set; }

        [RegularExpression("\\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\b",
            ErrorMessage = "Ip address is not a correct format.")]
        public string Ip { get; set; }
        
        public bool NmapStandard { get; set; }

        public bool NiktoStandard { get; set; }

        public bool XsserStandard { get; set; }

        public string ProjectId { get; set; }
    }

    public class CommandResult
    {
        public string Action { get; set; }

        public string NmapResult { get; set; }

        public string NiktoResult { get; set; }

        public string XsserResult { get; set; }

        public string Error { get; set; }
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

        [RegularExpression("\\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\b",
            ErrorMessage = "Ip address is not a correct format.")]
        public string Ip { get; set; }
    }
}
