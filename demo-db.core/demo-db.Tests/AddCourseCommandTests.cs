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

        //Test fails due to difference in the DateTime format
        //should be dd-mm-yyyy but it works only with mm-dd-yyyy - unknown reasons

        [TestMethod]
        public void ExecuteShouldCallServiceMethodAddCourseOnce()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);
            state.SetupGet(s => s.UserName).Returns("pesho");
           

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            var parameters = new string[] {"06-22-2012", "06-22-2013", "Alpha JS"};

            command.Execute((parameters));

            service.Verify(s => s.AddCourse("pesho", DateTime.Parse("06 - 22 - 2012"), DateTime.Parse("06-22-2013"), "Alpha JS"), Times.Once);
        }

        [TestMethod]
        public void ExecuteShouldCatchUserAlreadyExistsException()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);
            state.SetupGet(s => s.UserName).Returns("pesho");

            service.Setup(s =>
                    s.AddCourse("pesho", DateTime.Parse("06-22-2012"), DateTime.Parse("06-22-2013"), "Alpha JS"))
                .Throws(new UserAlreadyExistsException("Course already exists"));
               

            var command = new AddCourseCommand(state.Object, builder.Object, service.Object);

            var parameters = new string[] { "06-22-2012", "06-22-2013", "Alpha JS" };

            var str = command.Execute(parameters);

            Assert.AreEqual("Course already exists",str);
        }

    }
}
