using Frontend.Models;
using Frontend.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.MockClasses
{
    class MockUserDbContext : IUsersDbContext
    {
        private List<UserModel> Users = new List<UserModel>()
        {
            new UserModel
            {
                Id = "5cccc9fa67db7d35e88a5d9f",
                Username = "Mock sir Mocking",
                Email = "MockSir@Mock.nl",
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                Password = UserService.HashPassword("Mock sir Mocking", "Test1")
            },
            new UserModel
            {
                Id = "ddddd9fa67dddd35e88a5d9f",
                Username = "Ms Mocking Mock",
                Email = "MsMocking@Mock.nl",
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                Password = UserService.HashPassword("Ms Mocking Mock", "Test2")
            },
            new UserModel
            {
                Id = "eeeed9fa67dddd35e88a5d9f",
                Username = "Little Mocking Mock",
                Email = "LittleMocking@Mock.nl",
                TimeCreated = DateTime.Now,
                TimeLastEdit = DateTime.Now,
                Password = UserService.HashPassword("Little Mocking Mock", "Test3")
            }
        };

        // Not used in the mock, a list<UserModel> is used
        public IMongoCollection<UserModel> UsersCollection => throw new NotImplementedException();

        public void Delete(string id)
        {
            Users.Remove(Users.Find(x => x.Id == id));
        }

        public List<UserModel> Get()
        {
            return Users;
        }

        public UserModel Get(UserModel model)
        {
            var user = Users.Find(x => x.Username == model.Username &&
                x.Password == UserService.HashPassword(model.Username, model.Password));

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

        public UserModel Get(string id)
        {
            return Users.Find(x => x.Id == id);
        }

        public UserModel Post(UserModel model)
        {
            model.TimeCreated = DateTime.Now;
            model.TimeLastEdit = DateTime.Now;
            Users.Add(model);

            return model;
        }

        public UserModel Put(string id, UserModel model)
        {
            int index = Users.FindIndex(x => x.Id == id);
            Users[index] = model;

            return model;
        }
    }
}
