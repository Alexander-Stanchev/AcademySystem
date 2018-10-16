using demo_db.Common.Exceptions;
using demo_db.core.Contracts;
using demo_db.core.Commands;
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
using demo_db.Common.Wrappers;
using demo_db.Services.Abstract;

namespace demo_db.Tests
{
    [TestClass]
    public class UpdateStudentToTeacherCommandTest
    {
        [TestMethod]
        public void ExecuteShouldRetrurnMessageWhenUserNotLogged()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new UpdateStudentToTeacherCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "UserName", "1" };

            //Assert + Act
            Assert.AreEqual("You have to log in first.", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldRetrurnMessageWhenIncorrectRolePassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new UpdateStudentToTeacherCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(2);

            var parameters = new string[] { "UserName"};

            //Assert + Act
            Assert.AreEqual("You dont have access.", command.Execute(parameters));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteShouldThrowExceptionWhenNoUserNamePassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new UpdateStudentToTeacherCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] {""};

            var result = command.Execute(parameters);

            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteShouldThrowExceptionWhenMoreThanOneParametersPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new UpdateStudentToTeacherCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] { "username", "1234" };

            var result = command.Execute(parameters);
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenCommandExecuteSuccessfully()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new UpdateStudentToTeacherCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] { "username"};

            //Assert+Act
            Assert.AreEqual("User username role is updated.", command.Execute(parameters));
        }

    }
}
