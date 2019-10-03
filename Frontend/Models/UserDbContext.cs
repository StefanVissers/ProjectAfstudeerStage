using Frontend.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class UsersDbContext : IUsersDbContext
    {
        private readonly FilterDefinition<UserModel> EmptyUserFilter = Builders<UserModel>.Filter.Empty;

        MongoClient _client;
        public IMongoDatabase _database;
        private readonly MongoDBAppSettings Config;

        public UsersDbContext(MongoDBAppSettings config)
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
            MongoCredential credential = MongoCredential.CreateCredential
                (MongoAuthDatabaseName,
                 MongoUsername,
                 MongoPassword);

            // Connects to the Db.
            var settings = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);

            // Gets the corrext db.
            _database = _client.GetDatabase("Users");
        }

        public IEnumerable<UserModel> GetUsers(FilterDefinition<UserModel> filter = null, int? amount = null)
        {
            if (filter != null)
            {
                return UsersCollection.Find(filter).Limit(amount).ToList();
            }
            else
            {
                return UsersCollection.Find(EmptyUserFilter).Limit(amount).ToList();
            }
        }

        public UserModel Get(UserModel userModel)
        {
            var filterUsername = Builders<UserModel>.Filter.Eq(x => x.Username, userModel.Username);
            var filterPassword = Builders<UserModel>.Filter.Eq(x => x.Password, userModel.Password);
            var filterCombined = Builders<UserModel>.Filter.And(filterUsername, filterPassword);

            var user = UsersCollection.FindAsync(filterCombined).Result.FirstAsync().Result;

            return user;
        }

        public UserModel Get(string id)
        {
            var filterId = Builders<UserModel>.Filter.Eq(x => x.Id, id);
            var user = UsersCollection.FindAsync(filterId).Result.First();
            user.Password = null;
            return user;
        }

        public UserModel Post(UserModel user)
        {
            user.Password = UserService.HashPassword(user.Username, user.Password);

            UsersCollection.InsertOneAsync(user);

            return user;
        }

        public IMongoCollection<UserModel> UsersCollection
        {
            get
            {
                return _database.GetCollection<UserModel>("UserRecord");
            }
        }
    }

    interface IUsersDbContext : IDbContext<UserModel>
    {
        IMongoCollection<UserModel> UsersCollection { get; }
    }
}
