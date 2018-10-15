using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Commands;
using demo_db.core.Contracts;
using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories;
using demo_db.Services;
using demo_db.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Tests
{
    [TestClass]
    public class AddCourseCommandTests
    {
        [TestMethod]
        public void ExecuteShouldRetrurnMessageWhenUserNotLogged()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(false);

            var parameters = new string[] { "Username", "1" };

            //Assert + Act
            Assert.AreEqual("You will need to login first", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnWhenIncorrectRoleIsPassed()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(3);

            var parameters = new string[] { "Username" };

            //Assert + Act
            Assert.AreEqual("This command is available only to users with role Teacher", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnWhenCourseIsNull()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);

            var parameters = new string[] { "" };

            //Assert + Act
            Assert.AreEqual("The course name can`t be null", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldThrowWhenDateTimeIsIncorrect()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);
            
            var parameters = new string[] { "Alpha_JS", "pesho", "pesho" };
            //Assert + Act
            Assert.ThrowsException<Exception>(() => command.Execute(parameters));
        }
        [TestMethod]
        public void ExecuteShouldThrownWhenUserExists()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "ExecuteShouldThrownWhenUserExists")
                .Options;
            
            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);

            var course = new Course { CourseId = 1, Name = "Alpha JS" };
            var parameters = new string[] { "22-06-2012", "22-06-2013", "Alpha JS" };
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Courses.Add(course);
                dataHandler.SaveChanges();

                Assert.ThrowsException<UserAlreadyExistsException>(() => command.Execute(parameters));
                
            }
        }
    }
}
