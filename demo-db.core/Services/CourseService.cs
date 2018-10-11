using demo_db.Common.Exceptions;
using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace demo_db.Services
{
    public class CourseService : ICourseService
    {
        private readonly IDataHandler data;

        public CourseService(IDataHandler context)
        {
            this.data = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(string coursename, int teacherID, DateTime start, DateTime end)
        {
            var course = this.RetrieveCourse(coursename);
            if(course == null)
            {
                if(course.Teacher.RoleId == 2)
                {
                    course = new Course
                    {
                        Name = coursename,
                        TeacherId = teacherID,
                        Start = start,
                        End = end
                    };
                }
            }


        }

        public void EnrollStudent(string username, string coursename)
        {
            var user = this.data.Users.All().Include(us => us.EnrolledStudents).FirstOrDefault(us => us.UserName == username);
            var course = this.data.Courses.All().FirstOrDefault(co => co.Name == coursename);

            if(course == null)
            {
                throw new CourseDoesntExistsException("Unfortunately we are not offering such a course at the moment");
            }
            else if(user.EnrolledStudents.FirstOrDefault(es => es.CourseId == course.CourseId) != null)
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
        public IList<(string,string)> RetrieveCourseNames(string username = "")
        {
            var user = this.data.Users.All().FirstOrDefault(us => us.UserName == username);
            var userId = user.Id;
            var courses = this.data.Courses.All().Include(co => co.EnrolledStudents)
                .Include(co => co.Teacher)
                .Where(en => en.EnrolledStudents.Where(ec => ec.StudentId == userId).Count() == 0)
                .ToList();

            IList<(string, string)> returnValues = new List<(string, string)>();

            foreach(var course in courses)
            {
                returnValues.Add((course.Name, course.Teacher.FullName));
            }
            return returnValues;
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
