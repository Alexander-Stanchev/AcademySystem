using demo_db.Common.Exceptions;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services.Abstract;
using demo_db.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace demo_db.Services
{
    public class CourseService : ICourseService
    {
        private readonly IDataHandler data;
        private IUserService userService;

        public CourseService(IDataHandler context, IUserService userService)
        {
            this.data = context ?? throw new ArgumentNullException(nameof(context));
            this.userService = userService;
        }

        public void AddCourse(string coursename, string username, DateTime start, DateTime end)
        {
            Validations.ValidateLength(Validations.MIN_COURSENAME, Validations.MAX_COURSENAME, coursename, $"The course name can't be less than {Validations.MIN_COURSENAME} and greater than {Validations.MAX_COURSENAME}");

            var course = this.RetrieveCourse(coursename);

            var teacher = userService.RetrieveUser(username);

            if (teacher.RoleId != 2)
            {
                throw new ArgumentOutOfRangeException("You don't have access.");
            }

            if (course == null)
            {
                course = new Course
                {
                    Name = coursename,
                    TeacherId = teacher.Id,
                    Start = start,
                    End = end
                };
            }
            data.Courses.Add(course);
            data.SaveChanges();

        }

        public void EnrollStudent(string username, string coursename)
        {
            Validations.ValidateLength(Validations.MIN_USERNAME, Validations.MAX_USERNAME, username, $"The username can't be less than {Validations.MIN_USERNAME} and greater than {Validations.MAX_USERNAME}");
            Validations.ValidateLength(Validations.MIN_COURSENAME, Validations.MAX_COURSENAME, coursename, $"The course name can't be less than {Validations.MIN_COURSENAME} and greater than {Validations.MAX_COURSENAME}");
            Validations.VerifyUserName(username);

            var user = this.data.Users.All().Include(us => us.EnrolledStudents).FirstOrDefault(us => us.UserName == username);
            var course = this.data.Courses.All().FirstOrDefault(co => co.Name == coursename);

            if (course == null)
            {
                throw new CourseDoesntExistsException("Unfortunately we are not offering such a course at the moment");
            }
            else if (user.EnrolledStudents.FirstOrDefault(es => es.CourseId == course.CourseId) != null)
            {
                throw new CourseAlreadyEnrolledException($"You are already enrolled for the course {course.Name}.");
            }
            else
            {
                var enrolled = new EnrolledStudent
                {
                    StudentId = user.Id,
                    CourseId = course.CourseId
                };
                user.EnrolledStudents.Add(enrolled);
                this.data.SaveChanges();
            }

        }

        public IList<CourseViewModel> RetrieveCourseNames(int roleId, string username = "")
        {
            var user = this.data.Users.All().FirstOrDefault(us => us.UserName == username);
            var userId = user.Id;

            var courses = new List<Course>();

            if (roleId == 3)
            {
                courses = this.data.Courses.All().Include(co => co.EnrolledStudents)
                    .Include(co => co.Teacher)
                    .Where(en => en.EnrolledStudents.Where(ec => ec.StudentId == userId).Count() == 0)
                    .ToList();
            }
            else if (roleId == 2)
            {
                   courses = this.data.Courses.All().Include(co => co.EnrolledStudents)
                    .Include(co => co.Teacher)
                    .Where(c => c.TeacherId == user.Id)
                    //.Where(en => en.EnrolledStudents.Where(ec => ec.StudentId == userId).Count() == 0)
                    .ToList();
            }

            IList<CourseViewModel> returnValues = new List<CourseViewModel>();

            foreach (var course in courses)
            {
                returnValues.Add(new CourseViewModel { Teacher = new UserViewModel { FullName = course.Teacher.FullName }, CourseName = course.Name });
            }
            return returnValues;
        }
        public IList<GradeViewModel> RetrieveGrades(string username, string coursename = "")
        {
            var user = new User();
            if (coursename != "")
            {
                user = this.data.Users.All()
                    .Include(us => us.Grades)
                         .ThenInclude(gr => gr.Assaignment)
                            .ThenInclude(a => a.Course)
                    .FirstOrDefault(us => us.UserName == username && us.EnrolledStudents.Any(es => es.Course.Name == coursename));
            }
            else
            {
                user = this.data.Users.All()
                    .Include(us => us.Grades)
                        .ThenInclude(gr => gr.Assaignment)
                            .ThenInclude(a => a.Course)
                .FirstOrDefault(us => us.UserName == username);
            }

            if (user == null)
            {
                throw new NotEnrolledInCourseException("You are not assigned to this course");
            }

            IList<GradeViewModel> gradesMapped = new List<GradeViewModel>();

            foreach (var grade in user.Grades)
            {
                if (coursename == "")
                {
                    gradesMapped.Add(new GradeViewModel { Assaingment = new AssaignmentViewModel { Course = new CourseViewModel { CourseName = grade.Assaignment.Course.Name }, Name = grade.Assaignment.Name, MaxPoints = grade.Assaignment.MaxPoints }, Score = grade.ReceivedGrade });
                }
                else if (grade.Assaignment.Course.Name == coursename)
                {
                    gradesMapped.Add(new GradeViewModel { Assaingment = new AssaignmentViewModel { Course = new CourseViewModel { CourseName = grade.Assaignment.Course.Name }, Name = grade.Assaignment.Name, MaxPoints = grade.Assaignment.MaxPoints }, Score = grade.ReceivedGrade });
                }


            }
            return gradesMapped;

        }
        private Course RetrieveCourse(string coursename)
        {
            var course = this.data.Courses.All()
                .Include(co => co.Teacher)
                .FirstOrDefault(co => co.Name == coursename);

            return course;
        }

    }
}
