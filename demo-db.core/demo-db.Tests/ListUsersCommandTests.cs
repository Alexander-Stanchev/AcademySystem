using demo_db.Common.Enum;
using demo_db.Common.Wrappers;
using demo_db.core.Commands;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using demo_db.Services.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace demo_db.Tests
{
    [TestClass]
    public class ListUsersCommandTests
    {
        [TestMethod]
        public void ExecuteShouldReturnMessageWhenAlreadyLogged()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new ListUsersCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] { "2" };

            //Assert + Act
            Assert.AreEqual("Please log before using commands", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenNotCorrectUserLogged()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new ListUsersCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(2);

            var parameters = new string[] { "2" };

            Assert.AreEqual("This command is available only to users with role Admin", command.Execute(parameters));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteShouldThrowExceptionWhenNoParamsPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new ListUsersCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] { };

            var result = command.Execute(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteShouldThrowExceptionWhenNoPassPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new ListUsersCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);
            state.SetupGet(m => m.RoleId).Returns(1);

            var parameters = new string[] { };

            var users = new List<UserViewModel>();

            var result = command.Execute(parameters);
        }

    }
}
