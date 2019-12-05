using Frontend.Models;
using Frontend.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.MockClasses
{
    class MockUserService : IUserService
    {
        private readonly IUsersDbContext _userDbContext;

        public MockUserService(IUsersDbContext usersDbContext)
        {
            _userDbContext = usersDbContext;
        }

        public UserModel Authenticate(UserModel user)
        {
            var dbUser = _userDbContext.Get(user);

            dbUser.Token = "testToken";

            return dbUser;
        }
    }
}
