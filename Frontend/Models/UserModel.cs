using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
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
        public string Username { get; set; }

        [BsonElement()]
        public string Password { get; set; }

        [BsonElement()]
        public string Email { get; set; }
        
        [BsonElement()]
        public int Role { get; set; }

        [BsonElement()]
        public string Token { get; set; }
    }

}
