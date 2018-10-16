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
using System.Collections.Generic;
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
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            var username = "U";
            var password = "password";
            var fullname = "Pesho Goshov";

            //Assert + Act
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.AddUser(username, password, fullname));
        }

        [TestMethod]
        public void AddUserShouldThrowWhenInvalidPassword()
        {
            // Arrange 
            var mockRepository = new Mock<IDataHandler>();
            var sut = new UserService(mockRepository.Object);

            var username = "UserName";
            var password = "p";
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

                Assert.ThrowsException<EntityAlreadyExistsException>(() => sut.AddUser(username, password, fullname));
            }
        }

        [TestMethod]
        public void AddUserShouldAddWhenCorrectDataIsPassed()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "AddUserShouldAddWhenCorrectDataIsPassed")
                .Options;

            var username = "username";
            var password = "password";
            var fullname = "Pesho Goshov";

            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                var sut = new UserService(dataHandler);
                sut.AddUser(username, password, fullname);

                Assert.IsTrue(context.Users.Count() == 1);
                Assert.IsTrue(context.Users.Any(user => user.UserName == username && user.FullName == fullname));

            }
        }

        [TestMethod]
        public void RetrieveUserShouldReturnNullIfNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "AddUserShouldThrowWhenUsernameNotAvailable")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                var sut = new UserService(dataHandler);

                Assert.IsNull(sut.RetrieveUser("Pesho"));
            }
        }

        [TestMethod]
        public void RetrieveUserShouldReturnValidUserWhenFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveUserShouldReturnValidUserWhenFound")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                var sut = new UserService(dataHandler);
                sut.AddUser("Pesho", "pesho007", "Gosho Peshov");

                Assert.IsNotNull(sut.RetrieveUser("Pesho"));
                Assert.IsTrue(sut.RetrieveUser("Pesho").FullName == "Gosho Peshov");
            }
        }

        [TestMethod]
        public void LogInUserShouldThrowIfInvalidUserName()
        {
            //Arrange

            var username = "P";
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            //Act and Assert

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.LoginUser(username, "parola"));
        }

        [TestMethod]
        public void LogInUserShouldThrowIfRegexDoesntMatchUserName()
        {
            var username = "Pesho Gosho";
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            //Act and Assert

            Assert.ThrowsException<ArgumentException>(() => sut.LoginUser(username, "parola"));
        }

        [TestMethod]
        public void LogInUserShouldThrowIfUserDoesntExist()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "LogInUserShouldThrowIfUserDoesntExist")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);
                var sut = new UserService(dataHandler);
                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                Assert.ThrowsException<UserDoesntExistsException>(() => sut.LoginUser("pesho007", "parola"));
            }
        }

        [TestMethod]
        public void LogInShouldThrowWhenPasswordNotEqual()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "LogInShouldThrowWhenPasswordNotEqual")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };
            var user = new User { UserName = "pesho007", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);
                var sut = new UserService(dataHandler);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                dataHandler.Users.Add(user);
                dataHandler.SaveChanges();

                Assert.ThrowsException<InvalidPasswordException>(() => sut.LoginUser("pesho007", "parola2"));

            }

        }
        [TestMethod]
        public void LogInShouldReturnProperId()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "LogInShouldReturnProperId")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };
            var user = new User { UserName = "pesho007", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            int roleId;

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);
                var sut = new UserService(dataHandler);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                dataHandler.Users.Add(user);
                dataHandler.SaveChanges();

                roleId = sut.LoginUser("pesho007", "parola");

            }


            Assert.AreEqual(2, roleId);
        }

        [TestMethod]
        public void UpdateRoleShouldThrowIfInvalidUserName()
        {
            // Arrange 
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            var username = "U";
            var roleId = 2;


            //Assert + Act
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.UpdateRole(username, roleId));
        }

        [TestMethod]
        public void UpdateRoleShouldThrowIfUserNameDoesntMatchRegex()
        {
            // Arrange 
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            var username = "Uuu Uuu";
            var roleId = 2;


            //Assert + Act
            Assert.ThrowsException<ArgumentException>(() => sut.UpdateRole(username, roleId));
        }

        [TestMethod]
        public void UpdateUserShouldThrowInvalidOperationIfRoleIsAdmin()
        {
            // Arrange 
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            var username = "validuser";
            var roleId = 1;


            //Assert + Act
            Assert.ThrowsException<InvalidOperationException>(() => sut.UpdateRole(username, roleId));
        }

        [TestMethod]
        public void UpdateRoleShouldThrowIfUserNotFound()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRoleShouldThrowIfUserNotFound")
                .Options;

            var username = "username";
            var roleId = 2;
  

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

                Assert.ThrowsException<ArgumentNullException>(() => sut.UpdateRole(username, roleId));
            }
        }

        [TestMethod]
        public void UpdateRoleShouldUpdateProperlyIfCorrectParamsPassed()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "UpdateRoleShouldUpdateProperlyIfCorrectParamsPassed")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var user = new User { UserName = "pesho007", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(user);

                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);

                sut.UpdateRole("pesho007",3);

                var userRetrieved = dataHandler.Users.All().FirstOrDefault(us => us.UserName == "pesho007");

                Assert.AreEqual(3, userRetrieved.RoleId);
            }
        }

        [TestMethod]
        public void RetrieveUsersShouldReturnCorrectNumberOfUsers()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveUsersShouldReturnCorrectNumberOfUsers")
                .Options;


            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var user = new User { UserName = "pesho007", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            var user2 = new User { UserName = "pesho006", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 2, Role = studentRole };
            var user3 = new User { UserName = "pesho005", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 3, Role = studentRole };
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);

                dataHandler.Users.Add(user);
                dataHandler.Users.Add(user2);
                dataHandler.Users.Add(user3);

                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);


                var usersRetrieved = sut.RetrieveUsers(2);

                Assert.AreEqual(3, usersRetrieved.Count);
            }
        }

        [TestMethod]
        public void EvaluateStudentShouldThrowIfUserNameIsInvalid()
        {
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            string username = "P";
            int assignmentId = 2;
            int grade = 80;
            string teacherUsername = "PeshoPeshov";


            //Assert + Act
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.EvaluateStudent(username, assignmentId, grade, teacherUsername));
        }

        [TestMethod]
        public void EvaluateStudentShouldThrowIfUserNameDoesntMatchRegex()
        {
            var stubRepository = new Mock<IDataHandler>();
            var sut = new UserService(stubRepository.Object);

            string username = "Pes ho";
            int assignmentId = 2;
            int grade = 80;
            string teacherUsername = "PeshoPeshov";


            //Assert + Act
            Assert.ThrowsException<ArgumentException>(() => sut.EvaluateStudent(username, assignmentId, grade, teacherUsername));
        }

        [TestMethod]
        public void EvaluateStudentShouldThrowWhenAssaignmentNotFound()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowWhenAssaignmentNotFound")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var studentName = "pesho007";
            var teacherName = "teacher";

            var student = new User { UserName = studentName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 3, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            var teacher = new User { UserName = teacherName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 2, Role = studentRole };
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(student);
                dataHandler.Users.Add(teacher);

                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);

                Assert.ThrowsException<ArgumentNullException>(
                    () => sut.EvaluateStudent(studentName, 1, 80, teacherName));

            }
        }
        [TestMethod]
        public void EvaluateStudentShouldThrowWhenTeacherIsDifferentThanPassed()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowWhenTeacherIsDifferentThanPassed")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var studentName = "pesho007";
            var teacherName = "teacher2";

            var student = new User { UserName = studentName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 3, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            var teacher = new User { UserName = "teacher", Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 2, Role = studentRole };
            var invalidTeacher = new User { UserName = teacherName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 3, Role = studentRole };
            var assaignment = new Assaignment {Course = new Course {Teacher = teacher}, Id = 1};
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(student);
                dataHandler.Users.Add(teacher);
                dataHandler.Users.Add(invalidTeacher);
                dataHandler.Assaignments.Add((assaignment));
                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);

                Assert.ThrowsException<ArgumentException>(
                    () => sut.EvaluateStudent(studentName, 1, 80, teacherName));

            }
        }

        [TestMethod]
        public void EvaluateStudentShouldThrowWhenStudentIsEnrolledInCourse()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowWhenStudentIsEnrolledInCourse")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var studentName = "pesho007";
            var teacherName = "teacher2";

            var student = new User { UserName = studentName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 3, RegisteredOn = DateTime.Now, Id = 1, Role = studentRole };
            var teacher = new User { UserName = teacherName, Deleted = false, FullName = "Gosho Peshov", Password = "parola", RoleId = 2, RegisteredOn = DateTime.Now, Id = 2, Role = studentRole };
            
            var assaignment = new Assaignment { Course = new Course { Teacher = teacher }, Id = 1 };
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(student);
                dataHandler.Users.Add(teacher);

                dataHandler.Assaignments.Add((assaignment));
                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);

                Assert.ThrowsException<ArgumentException>(
                    () => sut.EvaluateStudent(studentName, 1, 80, teacherName));

            }
        }
        [TestMethod]
        public void EvaluateStudentShouldThrowWhenStudentAlreadyHasGradeForAssaignment()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldThrowWhenStudentAlreadyHasGradeForAssaignment")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var studentName = "pesho007";
            var teacherName = "teacher2";

            var student = new User
                {
                    UserName = studentName,
                    Deleted = false, FullName = "Gosho Peshov",
                    Password = "parola", RoleId = 3,
                    RegisteredOn = DateTime.Now,
                    Id = 1,
                    Role = studentRole,
                    Grades = new List<Grade>
                    {
                        new Grade{AssaignmentId = 1, ReceivedGrade = 88, StudentId = 1}
                    },
                    EnrolledStudents = new List<EnrolledStudent>()
                    {
                        new EnrolledStudent{CourseId = 1, StudentId = 1}
                    }
                    
                };

            var teacher = new User
            {
                UserName = teacherName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 2,
                RegisteredOn = DateTime.Now,
                Id = 2,
                Role = studentRole
            };

            var assaignment = new Assaignment
            {
                Course = new Course { Teacher = teacher, CourseId = 1},
                Id = 1,
                
            };
    
            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(student);
                dataHandler.Users.Add(teacher);

                dataHandler.Assaignments.Add((assaignment));
                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);

                Assert.ThrowsException<ArgumentException>(
                    () => sut.EvaluateStudent(studentName, 1, 80, teacherName));

            }
        }
        [TestMethod]
        public void EvaluateStudentShouldAddGradeWhenCorrectParametersArePassed()
        {
            // Arrange 
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EvaluateStudentShouldAddGradeWhenCorrectParametersArePassed")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var studentName = "pesho007";
            var teacherName = "teacher2";

            var student = new User
            {
                UserName = studentName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 3,
                RegisteredOn = DateTime.Now,
                Id = 1,
                Role = studentRole,
                EnrolledStudents = new List<EnrolledStudent>()
                    {
                        new EnrolledStudent{CourseId = 1, StudentId = 1}
                    }

            };

            var teacher = new User
            {
                UserName = teacherName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 2,
                RegisteredOn = DateTime.Now,
                Id = 2,
                Role = studentRole
            };

            var assaignment = new Assaignment
            {
                Course = new Course { Teacher = teacher, CourseId = 1 },
                Id = 1,

            };

            //Act + Assert
            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(student);
                dataHandler.Users.Add(teacher);

                dataHandler.Assaignments.Add((assaignment));
                dataHandler.SaveChanges();

                var sut = new UserService(dataHandler);
                sut.EvaluateStudent(studentName,1,88,teacherName);
                var studentRetrieved = dataHandler.Users.All().FirstOrDefault(us => us.UserName == studentName);

                Assert.IsTrue(studentRetrieved.Grades.Count == 1);
                Assert.IsTrue(studentRetrieved.Grades.Any(gr => gr.AssaignmentId == 1 && gr.ReceivedGrade == 88));


            }
        }
    }
}
