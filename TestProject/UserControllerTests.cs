using Frontend.Controllers;
using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using TestProject.MockClasses;

namespace TestProject
{
    [TestClass]
    public class UserControllerTests
    {
        private UserController _userController;

        public UserControllerTests()
        {
            // Create a ClaimsPrincipal to mock a logged in user.
            var identity = new GenericIdentity("5cccc9fa67db7d35e88a5d9f");
            var claimsIdentity = new ClaimsIdentity(identity);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            _userController = new UserController(new MockUserDbContext(), new MockUserService(new MockUserDbContext()));
            _userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };
        }

        [TestMethod]
        public void GetAllTest()
        {
            var response = _userController.Get();
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            var projects = okObjectResult.Value as List<UserModel>;
            Assert.IsNotNull(projects);
            Assert.AreEqual(projects.Count(), 3);
        }

        [DataTestMethod]
        [DataRow("eeeed9fa67dddd35e88a5d9f")]
        [DataRow("ddddd9fa67dddd35e88a5d9f")]
        [DataRow("5cccc9fa67db7d35e88a5d9f")]
        public void GetTest(string id)
        {
            var response = _userController.Get(id);
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var user = okObjectResult.Value as UserModel;
            Assert.IsNotNull(user);
            Assert.IsNull(user.Password);
        }

        [TestMethod]
        public void FromTokenTest()
        {
            var response = _userController.FromToken();
            Assert.IsNotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var user = okObjectResult.Value as UserModel;
            Assert.IsNotNull(user);
            Assert.AreEqual("Mock sir Mocking", user.Username);
            Assert.AreEqual("MockSir@Mock.nl", user.Email);
        }

        [TestMethod]
        public void PostTest()
        {
            var response = _userController.Get();
            var okObjectResult = response.Result as OkObjectResult;
            var projects = okObjectResult.Value as List<UserModel>;
            Assert.AreEqual(projects.Count(), 3);

            var old = new DateTime(2000, 6, 5);
            var user = new UserModel()
            {
                Email = "testMail@mail.nl",
                Password = "Password",
                Role = 0,
                TimeCreated = old,
                TimeLastEdit = old,
                Username = "testUsername"
            };
            var response2 = _userController.Post(user);
            Assert.IsNotNull(response2);
            okObjectResult = response2.Result as OkObjectResult;
            Assert.AreEqual(200, okObjectResult.StatusCode);
            var user2 = okObjectResult.Value as UserModel;
            Assert.AreNotEqual(user2.TimeCreated, old);
            Assert.AreNotEqual(user2.TimeLastEdit, old);
            Assert.AreEqual(user.Username, user2.Username);
            Assert.AreEqual(user.Email, user2.Email);

            response = _userController.Get();
            okObjectResult = response.Result as OkObjectResult;
            projects = okObjectResult.Value as List<UserModel>;
            Assert.AreEqual(projects.Count(), 4);
        }

        [DataTestMethod]
        [DataRow("eeeed9fa67dddd35e88a5d9f")]
        [DataRow("ddddd9fa67dddd35e88a5d9f")]
        [DataRow("5cccc9fa67db7d35e88a5d9f")]
        public void DeleteTest(string id)
        {
            var response = _userController.Get();
            var okObjectResult = response.Result as OkObjectResult;
            var projects = okObjectResult.Value as List<UserModel>;
            Assert.AreEqual(3, projects.Count());

            var response2 = _userController.Delete(id);

            response = _userController.Get();
            okObjectResult = response.Result as OkObjectResult;
            projects = okObjectResult.Value as List<UserModel>;
            Assert.AreEqual(2, projects.Count());
        }
    }
}
