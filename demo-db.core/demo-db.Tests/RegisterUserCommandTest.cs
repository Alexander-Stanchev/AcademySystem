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
    public class RegisterUserCommandTest
    {
        [TestMethod]
        public void ExecuteShouldRetrurnMessageWhenUserLogged()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new RegisterUserCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(true);

            var parameters = new string[] { "Username", "pass", "First", "Last" };

            //Assert + Act
            Assert.AreEqual("You are already logged.", command.Execute(parameters));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteShouldThrowExceptionWhenLessThan4ParametersPassed()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new RegisterUserCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);
            

            var parameters = new string[] { "username", "1234", "asd" };

            var result = command.Execute(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteShouldThrowExceptionWhenUsernameIsNull()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new RegisterUserCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);
            
            var parameters = new string[] { "", "1234", "First", "Last"};

            var result = command.Execute(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteShouldThrowExceptionWhenPasswordIsNull()
        {
            // Arrange 
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<IUserService>();

            var command = new RegisterUserCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(m => m.IsLogged).Returns(false);

            var parameters = new string[] { "userName", "", "First", "Last" };

            var result = command.Execute(parameters);
        }

    }
}
