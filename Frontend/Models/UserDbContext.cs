using Frontend.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frontend.Models
{
    public class UsersDbContext : IUsersDbContext
    {
        private readonly FilterDefinition<UserModel> EmptyUserFilter = Builders<UserModel>.Filter.Empty;

        MongoClient _client;
        public IMongoDatabase _database;
        private readonly MongoDBAppSettings Config;

        public UsersDbContext(IOptions<MongoDBAppSettings> config)
        {
            // Uses the config.
            Config = config.Value;

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

        /// <summary>
        /// Get a user based on the Username and Passwordhash.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public UserModel Get(UserModel userModel)
        {
            userModel.Password = UserService.HashPassword(userModel.Username, userModel.Password);

            var filterUsername = Builders<UserModel>.Filter.Eq(x => x.Username, userModel.Username);
            var filterPassword = Builders<UserModel>.Filter.Eq(x => x.Password, userModel.Password);
            var filterCombined = Builders<UserModel>.Filter.And(filterUsername, filterPassword);

            var user = UsersCollection.FindAsync(filterCombined).Result.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get a user based on the userId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel Get(string id)
        {
            var filterId = Builders<UserModel>.Filter.Eq(x => x.Id, id);
            var user = UsersCollection.FindAsync(filterId).Result.FirstOrDefault();

            if (user != null)
            {
                user.Password = null;
            } 
            else
            {
                user = new UserModel();
            }

            return user;
        }

        /// <summary>
        /// Gets a list of users with only its Id, Username and Email.
        /// </summary>
        /// <returns></returns>
        public List<UserModel> Get()
        {
            var users = UsersCollection.Find(EmptyUserFilter).ToList();

            var userss = users.Select(x => new UserModel() { Id = x.Id, Username = x.Username, Email = x.Email }).ToList();

            return userss;
        }

        /// <summary>
        /// Insert a UserModel into the database. Also hashes the password before inserting.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserModel Post(UserModel user)
        {
            user.TimeCreated = DateTime.Now;
            user.TimeLastEdit = DateTime.Now;
            user.Password = UserService.HashPassword(user.Username, user.Password);

            if (Get(user) == null)
            {
                UsersCollection.InsertOne(user);
                user.Password = null;
                return user;
            }
            else
            {
                user.Password = null;
                return user;
            }
        }

        /// <summary>
        /// Updates a users username, password and email.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserModel Put(string id, UserModel model)
        {
            model.TimeLastEdit = DateTime.Now;
            model.Password = UserService.HashPassword(model.Username, model.Password);

            var filterId = Builders<UserModel>.Filter.Eq(x => x.Id, id);

            var updateUserName = Builders<UserModel>.Update.Set(x => x.Username, model.Username);
            var updateUserPassword = Builders<UserModel>.Update.Set(x => x.Password, model.Password);
            var updateUserEmail = Builders<UserModel>.Update.Set(x => x.Email, model.Email);
            var updateUserTimeLastEdit = Builders<UserModel>.Update.Set(x => x.TimeLastEdit, model.TimeLastEdit);

            var updates = Builders<UserModel>.Update.Combine(updateUserEmail, updateUserName, updateUserPassword);

            return UsersCollection.FindOneAndUpdate(filterId, updates);
        }

        /// <summary>
        /// Deletes a User using its Id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            var filterId = Builders<UserModel>.Filter.Eq(x => x.Id, id);

            UsersCollection.FindOneAndDelete(filterId);
        }

        public IMongoCollection<UserModel> UsersCollection
        {
            get
            {
                return _database.GetCollection<UserModel>("UserRecord");
            }
        }
    }

    public interface IUsersDbContext : IDbContext<UserModel>
    {
        List<UserModel> Get();
        UserModel Get(UserModel model);
        IMongoCollection<UserModel> UsersCollection { get; }
    }
}
