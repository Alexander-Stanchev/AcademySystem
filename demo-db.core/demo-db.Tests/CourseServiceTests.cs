using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using demo_db.Common.Exceptions;
using demo_db.Data.Context;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services;
using demo_db.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace demo_db.Tests
{
    [TestClass]
    public class CourseServiceTests
    {
        [TestMethod]
        public void CreateCourseShouldThrowIfInvalidName()
        {
            var coursname = "p";

            var stubRepository = new Mock<IDataHandler>();
            var stubUserService = new Mock<IUserService>();

            var sut = new CourseService(stubRepository.Object, stubUserService.Object);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                sut.AddCourse("Pesho", DateTime.MinValue, DateTime.MaxValue, coursname));
        }

        [TestMethod]
        public void CreateCourseShouldThrowIfInvalidUserPermissions()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "CreateCourseShouldThrowIfInvalidUserPermissions")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };
            var teacherName = "pesho007";

            var user = new User
            {
                UserName = teacherName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 3,
                RegisteredOn = DateTime.Now,
                Id = 1,
                Role = studentRole
            };

            var userServiceStub = new Mock<IUserService>();
            userServiceStub.Setup(us => us.RetrieveUser(teacherName)).Returns(user);

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(user);

                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                    sut.AddCourse(teacherName, DateTime.MinValue, DateTime.MaxValue, "coursname"));

            }

        }

        [TestMethod]
        public void CreateCourseShouldThrowIfCourseExists()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "CreateCourseShouldThrowIfCourseExists")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var teacherName = "pesho007";
            var courseName = "SoftSkills";

            var user = new User
            {
                UserName = teacherName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 2,
                RegisteredOn = DateTime.Now,
                Id = 1,
                Role = studentRole
            };

            var course = new Course
            {
                CourseId = 1,
                Name = courseName,
                Teacher = user
            };

            var userServiceStub = new Mock<IUserService>();
            userServiceStub.Setup(us => us.RetrieveUser(teacherName)).Returns(user);

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(user);
                dataHandler.Courses.Add(course);

                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<UserAlreadyExistsException>(() =>
                    sut.AddCourse(teacherName, DateTime.MinValue, DateTime.MaxValue, courseName));

            }

        }

        [TestMethod]
        public void CreateCourseShouldAddNewCourseIfAllParametersAreCorrect()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "CreateCourseShouldAddNewCourseIfAllParametersAreCorrect")
                .Options;

            //Setup roles for the in-memory database
            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var teacherName = "pesho007";
            var courseName = "SoftSkills";

            var user = new User
            {
                UserName = teacherName,
                Deleted = false,
                FullName = "Gosho Peshov",
                Password = "parola",
                RoleId = 2,
                RegisteredOn = DateTime.Now,
                Id = 1,
                Role = studentRole
            };


            var userServiceStub = new Mock<IUserService>();
            userServiceStub.Setup(us => us.RetrieveUser(teacherName)).Returns(user);

            using (var context = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(context);

                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.Users.Add(user);

                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                sut.AddCourse(teacherName, DateTime.MinValue, DateTime.MaxValue, courseName);

            }

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);

                Assert.IsTrue(dataHandler.Courses.All().Count() == 1);
                Assert.IsTrue(dataHandler.Courses.All().Any(co => co.Name == courseName));

            }

        }

        [TestMethod]
        public void EnrollStudentShouldThrowIfInvalidUserName()
        {
            var userName = "p";
            var courseName = "c#Advanced";

            var contextStub = new Mock<IDataHandler>();
            var userServiceStub = new Mock<IUserService>();

            var sut = new CourseService(contextStub.Object, userServiceStub.Object);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.EnrollStudent(userName, courseName));
        }

        [TestMethod]
        public void EnrollStudentShouldThrowIfUserNameDoesntMatchRegext()
        {
            var userName = "pesho losta";
            var courseName = "c#Advanced";

            var contextStub = new Mock<IDataHandler>();
            var userServiceStub = new Mock<IUserService>();

            var sut = new CourseService(contextStub.Object, userServiceStub.Object);

            Assert.ThrowsException<ArgumentException>(() => sut.EnrollStudent(userName, courseName));
        }

        [TestMethod]
        public void EnrollStudentShouldThrowIfInvalidCourseName()
        {
            var userName = "pesho";
            var courseName = "c#";

            var contextStub = new Mock<IDataHandler>();
            var userServiceStub = new Mock<IUserService>();

            var sut = new CourseService(contextStub.Object, userServiceStub.Object);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sut.EnrollStudent(userName, courseName));
        }

        [TestMethod]
        public void EnrollStudentShouldThrowIfCourseNotFound()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentShouldThrowIfCourseNotFound")
                .Options;

            var studentName = "pesho007";
            var courseName = "SoftSkills";

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<CourseDoesntExistsException>(() => sut.EnrollStudent(studentName, courseName));
            }

        }

        [TestMethod]
        public void EnrollStudentShouldThrowUserAlreadyEnrolled()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentShouldThrowUserAlreadyEnrolled")
                .Options;

            var studentName = "pesho007";
            var courseName = "SoftSkills";

            var course = new Course
            {
                CourseId = 1,
                Name = courseName
            };

            var user = new User
            {
                UserName = studentName,
                EnrolledStudents = new List<EnrolledStudent> { new EnrolledStudent {Course = course, CourseId = 1} }
            };

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Courses.Add(course);
                dataHandler.Users.Add(user);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<CourseAlreadyEnrolledException>(() => sut.EnrollStudent(studentName, courseName));
            }

        }

        [TestMethod]
        public void EnrollStudentShouldCorrectlyEnrollIfValidParams()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "EnrollStudentShouldCorrectlyEnrollIfValidParams")
                .Options;

            var studentName = "pesho007";
            var courseName = "SoftSkills";

            var adminRole = new Role { Id = 1, Name = "Administrator" };
            var teacherRole = new Role { Id = 2, Name = "Teacher" };
            var studentRole = new Role { Id = 3, Name = "Student" };

            var course = new Course
            {
                CourseId = 1,
                Name = courseName
            };

            var user = new User
            {
                UserName = studentName,
                RoleId = 3,
                Role = studentRole,
                Id = 1
            };

            var userServiceStub = new Mock<IUserService>();

            using (var actContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(actContext);
                dataHandler.Courses.Add(course);
                dataHandler.Users.Add(user);
                dataHandler.Roles.Add(adminRole);
                dataHandler.Roles.Add(teacherRole);
                dataHandler.Roles.Add(studentRole);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                sut.EnrollStudent(studentName,courseName);
            }

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                var userRetrieved = dataHandler.Users.All().Include(us => us.EnrolledStudents).FirstOrDefault(us => us.UserName == studentName);

                Assert.IsTrue(userRetrieved.EnrolledStudents.Count() == 1);

            }

        }

        [TestMethod]
        public void RetrieveCourseNamesShouldReturnCorrectCoursesForStudent()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCourseNamesShouldReturnCorrectCoursesForStudent")
                .Options;

            var studentName = "Pesho";

            var user = new User
            {
                Id = 1,
                UserName = studentName,
                RoleId = 3
                
            };


            var courseEnrolled = new Course
            {
                CourseId = 1,
                Name = "C# Alpha",
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 1,
                        StudentId = 1
                    }
                },
                Teacher = new User { Id = 2}
            };

            var courseNotEnrolled = new Course
            {
                CourseId = 2,
                Name = "C++ Alpha",
                Teacher = new User { Id =2}

            };

            var userServiceStub = new Mock<IUserService>();

            using (var arrangeContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(arrangeContext);
                dataHandler.Courses.Add(courseEnrolled);
                dataHandler.Courses.Add(courseNotEnrolled);
                dataHandler.Users.Add(user);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                var courses = sut.RetrieveCourseNames(user.RoleId, user.UserName);
                Assert.IsTrue(courses.Count == 1);
                Assert.IsTrue(courses.Any(co => co.CourseName == "C++ Alpha"));
            }
        }

        [TestMethod]
        public void RetrieveCourseNamesShouldReturnCorrectCoursesForTeacher()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveCourseNamesShouldReturnCorrectCoursesForTeacher")
                .Options;

            var teacherName = "Pesho";

            var user = new User
            {
                Id = 1,
                UserName = teacherName,
                RoleId = 2

            };


            var courseEnrolled = new Course
            {
                CourseId = 1,
                Name = "C# Alpha",
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 1,
                        StudentId = 1
                    }
                },
                Teacher = user
            };

            var courseNotEnrolled = new Course
            {
                CourseId = 2,
                Name = "C++ Alpha",
                Teacher = new User { Id = 2 }

            };

            var userServiceStub = new Mock<IUserService>();

            using (var arrangeContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(arrangeContext);
                dataHandler.Courses.Add(courseEnrolled);
                dataHandler.Courses.Add(courseNotEnrolled);
                dataHandler.Users.Add(user);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                var courses = sut.RetrieveCourseNames(user.RoleId, user.UserName);
                Assert.IsTrue(courses.Count == 1);
                Assert.IsTrue(courses.Any(co => co.CourseName == "C# Alpha"));
            }
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfInvalidUsername()
        {
            var userName = "P";
            var coursname = "Python";

            var stubRepository = new Mock<IDataHandler>();
            var stubUserService = new Mock<IUserService>();

            var sut = new CourseService(stubRepository.Object, stubUserService.Object);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                sut.RetrieveStudentsInCourse(coursname, 2, userName));
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfInvalidCourseName()
        {
            var userName = "Pesho";
            var coursname = "Py";

            var stubRepository = new Mock<IDataHandler>();
            var stubUserService = new Mock<IUserService>();

            var sut = new CourseService(stubRepository.Object, stubUserService.Object);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                sut.RetrieveStudentsInCourse(coursname, 2, userName));
        }

        [TestMethod]
        public void RetrieveStudentsInCourseShouldThrowIfRegexFailsUsername()
        {
            var userName = "Pes ho";
            var coursname = "Python";

            var stubRepository = new Mock<IDataHandler>();
            var stubUserService = new Mock<IUserService>();

            var sut = new CourseService(stubRepository.Object, stubUserService.Object);

            Assert.ThrowsException<ArgumentException>(() =>
                sut.RetrieveStudentsInCourse(coursname, 2, userName));
        }

        [TestMethod]
        public void RetrieveStudentsShouldThrowIfCourseIsNull()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsShouldThrowIfCourseIsNull")
                .Options;

            var userName = "Pesho";
            var courseName = "Python";
            var roleId = 2;

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<ArgumentNullException>(() => sut.RetrieveStudentsInCourse(courseName, roleId, userName));
            }

        }

        [TestMethod]
        public void RetrieveStudentsShouldThrowIfNotTeacherOfGivenCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsShouldThrowIfNotTeacherOfGivenCourse")
                .Options;

            var userName = "Pesho";
            var courseName = "Python";
            var roleId = 2;

            var userServiceStub = new Mock<IUserService>();
            var course = new Course
            {
                Teacher = new User
                {
                    Id = 1,
                    RoleId = 2

                },
                CourseId = 1,
                Name = courseName
            };

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Courses.Add(course);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);

                Assert.ThrowsException<ArgumentNullException>(() => sut.RetrieveStudentsInCourse(courseName, roleId, userName));
            }

        }

        [TestMethod]
        public void RetrieveStudentsShouldReturnCorrectStudents()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentsShouldReturnCorrectStudents")
                .Options;

            var userName = "Pesho";
            var courseName = "Python";
            var roleId = 2;

            var userServiceStub = new Mock<IUserService>();
            var course = new Course
            {
                Teacher = new User
                {
                    Id = 1,
                    RoleId = 2,
                    UserName = userName

                },
                CourseId = 1,
                Name = courseName,
            };

            var userInCourse = new User
            {
                Id = 2,
                UserName = "pgoshov",
                FullName = "Pesho Goshov",
                RoleId = 3,
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 1,
                        StudentId = 2
                    }
                }
            };

            var userNotInCourse = new User
            {
                Id = 3,
                UserName = "gpeshov",
                FullName = "Pesho Goshov",
                RoleId = 3
            };

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Courses.Add(course);
                dataHandler.Users.Add(userInCourse);
                dataHandler.Users.Add(userNotInCourse);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                var res = sut.RetrieveStudentsInCourse(courseName, 2, userName);

                Assert.AreEqual(1,res.Count);
                Assert.IsTrue(res.Any(st => st.Username == "pgoshov"));

            }

        }

        [TestMethod]
        public void RetrieveStudentGradesShouldReturnCorrectWhenCourseNameIsGiven()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentGradesShouldReturnCorrectWhenCourseNameIsGiven")
                .Options;

            var username = "pesho";
            var coursename = "Python";

            var course = new Course
            {
                CourseId = 1,
                Teacher = new User { UserName = "teacher",RoleId = 2,Id = 2},
                TeacherId = 2,
                Name = coursename,

            };

            var notSearchedCourse = new Course
            {
                CourseId = 2,
                Teacher = new User {UserName = "teacher", RoleId = 2, Id = 3},
                TeacherId = 2,
                Name = "CSSBasics"
            };

            var student = new User
            {
                UserName = username,
                Id = 1,
                RoleId = 3,
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 1,
                        Course = course,
                        StudentId = 1
                    },
                    new EnrolledStudent
                    {
                        CourseId = 2,
                        Course = notSearchedCourse,
                        StudentId = 1
                    }
                },
                Grades = new List<Grade>
                {
                    new Grade
                    {
                        StudentId = 1,
                        ReceivedGrade = 88,
                        AssaignmentId = 1,
                        Assaignment = new Assaignment{Course = course,CourseId = 1,Id=1}
                    },
                    new Grade
                    {
                        StudentId = 1,
                        ReceivedGrade = 98,
                        AssaignmentId = 2,
                        Assaignment = new Assaignment{Course = course,CourseId = 2,Id = 2}
                    }
                }
                
            };

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Users.Add(student);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                var result = sut.RetrieveGrades(username, coursename);

                Assert.AreEqual(1,result.Count);
                Assert.IsTrue(result.Any(gr => gr.Assaingment.Course.CourseName == coursename));


            }
        }

        [TestMethod]
        public void RetrieveStudentGradesShouldReturnAllGradesWhenCourseNameIsEmpty()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentGradesShouldReturnAllGradesWhenCourseNameIsEmpty")
                .Options;

            var username = "pesho";

            var course = new Course
            {
                CourseId = 1,
                Teacher = new User { UserName = "teacher", RoleId = 2, Id = 2 },
                TeacherId = 2,
                Name = "Python"
            };

            var notSearchedCourse = new Course
            {
                CourseId = 2,
                Teacher = new User { UserName = "teacher", RoleId = 2, Id = 3 },
                TeacherId = 2,
                Name = "CSSBasics"
            };

            var student = new User
            {
                UserName = username,
                Id = 1,
                RoleId = 3,
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 1,
                        Course = course,
                        StudentId = 1
                    },
                    new EnrolledStudent
                    {
                        CourseId = 2,
                        Course = notSearchedCourse,
                        StudentId = 1
                    }
                },
                Grades = new List<Grade>
                {
                    new Grade
                    {
                        StudentId = 1,
                        ReceivedGrade = 88,
                        AssaignmentId = 1,
                        Assaignment = new Assaignment{Course = course,CourseId = 1,Id=1}
                    },
                    new Grade
                    {
                        StudentId = 1,
                        ReceivedGrade = 98,
                        AssaignmentId = 2,
                        Assaignment = new Assaignment{Course = course,CourseId = 2,Id = 2}
                    }
                }

            };

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Users.Add(student);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                var result = sut.RetrieveGrades(username);

                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Any(gr => gr.Assaingment.Course.CourseName == "Python") 
                              && (result.Any(gr => gr.Assaingment.Course.CourseName == "CSSBasics")));


            }
        }

        [TestMethod]
        public void RetrieveStudentGradesShouldThrowWhenUserNotAssaignedToCourse()
        {
            var contextOptions = new DbContextOptionsBuilder<AcademyContext>()
                .UseInMemoryDatabase(databaseName: "RetrieveStudentGradesShouldThrowWhenUserNotAssaignedToCourse")
                .Options;

            var username = "pesho";
            var courseName = "Python";
            var course = new Course
            {
                CourseId = 1,
                Teacher = new User { UserName = "teacher", RoleId = 2, Id = 2 },
                TeacherId = 2,
                Name = courseName
            };

            var notSearchedCourse = new Course
            {
                CourseId = 2,
                Teacher = new User { UserName = "teacher", RoleId = 2, Id = 3 },
                TeacherId = 2,
                Name = "CSSBasics"
            };

            var student = new User
            {
                UserName = username,
                Id = 1,
                RoleId = 3,
                EnrolledStudents = new List<EnrolledStudent>
                {
                    new EnrolledStudent
                    {
                        CourseId = 2,
                        Course = notSearchedCourse,
                        StudentId = 1
                    }
                }
            };

            var userServiceStub = new Mock<IUserService>();

            using (var assertContext = new AcademyContext(contextOptions))
            {
                var dataHandler = new DataHandler(assertContext);
                dataHandler.Users.Add(student);
                dataHandler.SaveChanges();

                var sut = new CourseService(dataHandler, userServiceStub.Object);
                Assert.ThrowsException<NotEnrolledInCourseException>(() => sut.RetrieveGrades(username, courseName));
            }
        }
    }
}
