using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement()]
        [MinLength(4)]
        public string Username { get; set; }

        [BsonElement()]
        [MinLength(6)]
        public string Password { get; set; }

        [BsonElement()]
        [EmailAddress]
        public string Email { get; set; }

        [BsonElement()]
        public int Role { get; set; }

        [BsonElement()]
        public string Token { get; set; }

        // Meta Data
        [BsonElement()]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TimeCreated { get; set; }

        [BsonElement()]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TimeLastEdit { get; set; }
    }

    public class UpdateUserModel
    {
        public string Id { get; set; }

        [MinLength(4)]
        public string Username { get; set; }

        [MinLength(6)]
        public string OldPassword { get; set; }

        [MinLength(6)]
        public string NewPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class Response
    {
        public string Status { set; get; }
        public string Message { set; get; }
    }
}
