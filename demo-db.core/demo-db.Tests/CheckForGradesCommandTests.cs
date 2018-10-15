using demo_db.Common.Wrappers;
using demo_db.core.Commands;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using demo_db.Services.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Tests
{
    [TestClass]
    public class CheckForGradesCommandTests
    {
        [TestMethod]
        public void ExecuteShouldRetrurnMessageWhenUserNotLogged()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new CheckGradesForCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(false);

            var parameters = new string[] { "" };

            //Assert + Act
            Assert.AreEqual("You will need to login first", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenIncorrectRoleIsPassed()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new CheckGradesForCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(2);

            var parameters = new string[] { "Username" };

            //Assert + Act
            Assert.AreEqual("This command is available only to users with role Student", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldThrowWhenCourseNameIsNull()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new CheckGradesForCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(3);

            var parameters = new string[] { "" };

            //Assert + Act
            Assert.AreEqual("The course name can`t be null", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnWhenThereAreNoGrades()
        {
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new CheckGradesForCourseCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(true);
            state.Setup(s => s.RoleId).Returns(3);
            state.SetupGet(s => s.UserName).Returns("pesho");

            service.Setup(s => s.RetrieveGrades("pesho", "Alpha JS")).Returns(new List<GradeViewModel>());

            var parameters = new string[] { "Alpha JS" };

            Assert.AreEqual("No grades available for the course Alpha JS", command.Execute(parameters));
        }    
    }
}
