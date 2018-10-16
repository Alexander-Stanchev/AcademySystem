using System;
using System.Collections.Generic;
using System.Text;
using demo_db.core.Commands;
using demo_db.core.Contracts;
using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.Services.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace demo_db.Tests
{
    [TestClass]
    public class EnrollStudentCommandTests
    {
        [TestMethod]
        public void ExecuteShouldReturnMessageWhenUserNotLogged()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.Setup(s => s.IsLogged).Returns(false);

            var parameters = new string[] { "CourseName" };

            //Assert + Act
            Assert.AreEqual("Please log before using commands", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenUserNotStudent()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(s => s.IsLogged).Returns(true);
            state.SetupGet(s => s.RoleId).Returns(2);

            var parameters = new string[] { "CourseName" };

            //Assert + Act
            Assert.AreEqual("This command is available only to users with role Student", command.Execute(parameters));
        }

        [TestMethod]
        public void ExecuteShouldReturnMessageWhenParametersEmpty()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(s => s.IsLogged).Returns(true);
            state.SetupGet(s => s.RoleId).Returns(3);

            var parameters = new string[]{};

            //Assert + Act
            Assert.AreEqual("Please enter a valid course name", command.Execute(parameters));
        }


        [TestMethod]
        public void ExecuteShouldInvokeServiceMethodOnceWithCorrectParams()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(s => s.IsLogged).Returns(true);
            state.SetupGet(s => s.RoleId).Returns(3);
            state.SetupGet(s => s.UserName).Returns("Pesho");

            var parameters = new string[] {"Coursename" };
            command.Execute(parameters);
            //Assert + Act
            service.Verify(s => s.EnrollStudent("Pesho", "Coursename"), Times.Once );
        }

        [TestMethod]
        public void ExecuteShouldReturnExceptionMessageIfCourseDoesntExist()
        {
            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(s => s.IsLogged).Returns(true);
            state.SetupGet(s => s.RoleId).Returns(3);
            state.SetupGet(s => s.UserName).Returns("Pesho");

            service.Setup(s => s.EnrollStudent("Pesho", "Coursename"))
                .Throws(new CourseDoesntExistsException("Unfortunately we are not offering such a course at the moment"));

            var parameters = new string[] { "Coursename" };
            var result = command.Execute(parameters);

            Assert.AreEqual("Unfortunately we are not offering such a course at the moment",result);
        }

        [TestMethod]
        public void ExecuteShouldReturnExceptionMessageIfCourseAlreadyNrolled()
        {
            // CourseAlreadyEnrolledException

            //Arrange
            var state = new Mock<ISessionState>();
            var builder = new Mock<IStringBuilderWrapper>();
            var service = new Mock<ICourseService>();

            var command = new EnrollStudentCommand(state.Object, builder.Object, service.Object);

            state.SetupGet(s => s.IsLogged).Returns(true);
            state.SetupGet(s => s.RoleId).Returns(3);
            state.SetupGet(s => s.UserName).Returns("Pesho");

            service.Setup(s => s.EnrollStudent("Pesho", "Coursename"))
                .Throws(new CourseAlreadyEnrolledException("You are already enrolled for the course Coursename."));

            var parameters = new string[] { "Coursename" };
            var result = command.Execute(parameters);

            Assert.AreEqual("You are already enrolled for the course Coursename.", result);
        }
    }
}
