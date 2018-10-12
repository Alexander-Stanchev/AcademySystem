using demo_db.Common.Exceptions;
using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace demo_db.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void AddUserShouldThrowWhenInvalidUserName()
        {
            // Arrange 
            var mockRepository = new Mock<IDataHandler>();
            var sut = new UserService(mockRepository.Object);

            var username = "U";
            var password = "password";
            var fullname = "Pesho Goshov";

            //Assert + Act

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.AddUser(username, password, fullname));

        }

        [TestMethod]
        public void AddUserShouldThrowWhenInvalidFullName()
        {
            // Arrange 
            var mockRepository = new Mock<IDataHandler>();
            var sut = new UserService(mockRepository.Object);

            var username = "UserName";
            var password = "password";
            var fullname = "P";

            //Assert + Act

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.AddUser(username, password, fullname));

        }

        [TestMethod]
        public void AddUserShouldThrowWhenUsernameNotAvailable()
        {
            // Arrange 

            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "AddUserShouldThrowWhenUsernameNotAvailable")
                .Options;

            var username = "username";
            var password = "password";
            var fullname = "Pesho Goshov";

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            //Act + Assert

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                var sut = new UserService(dataHandler);
                sut.AddUser(username, password, fullname);
                
            }

            using(var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                var sut = new UserService(dataHandler);
                Assert.ThrowsException<UserAlreadyExistsException>(() => sut.AddUser(username, password, fullname));
            }
        }
    }
}
