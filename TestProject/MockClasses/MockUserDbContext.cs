using Frontend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.MockClasses
{
    class MockUserDbContext : IUsersDbContext
    {
        public IMongoCollection<UserModel> UsersCollection => throw new NotImplementedException();

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> Get()
        {
            throw new NotImplementedException();
        }

        public UserModel Get(UserModel model)
        {
            throw new NotImplementedException();
        }

        public UserModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public UserModel Post(UserModel model)
        {
            throw new NotImplementedException();
        }

        public UserModel Put(string id, UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
