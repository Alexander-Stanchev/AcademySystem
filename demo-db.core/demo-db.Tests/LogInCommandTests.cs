using demo_db.Common.Enum;
using demo_db.Common.Wrappers;
using demo_db.core.Commands;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Tests
{
    [TestClass]
    public class LogInCommandTests
    {
        [TestMethod]
        public void ExecuteShouldReturnMessageWhenAlreadyLogged()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new LogInCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);

            var parameters = new string[] { "UserName", "12345" };

            //Assert + Act
            Assert.AreEqual("You are already logged.", command.Execute(parameters));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteShouldThrowExceptionWhenLessThan2ParametersPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new LogInCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "asd" };

            var result = command.Execute(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteShouldThrowExceptionWhenNoUsernamePassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new LogInCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "", "12345" };

            var result = command.Execute(parameters);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteShouldThrowExceptionWhenNoPassPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new LogInCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "username", "" };

            var result = command.Execute(parameters);
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenCommandExecuteSuccessfully()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new LogInCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "username", "12345" };

            //Assert+Act
            Assert.AreEqual("User username succesfully logged. Your role is -1", command.Execute(parameters));
        }

    }
}
