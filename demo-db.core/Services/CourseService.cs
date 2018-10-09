using demo_db.Data.DataModels;
using demo_db.Data.Repositories.Contracts;
using demo_db.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo_db.Services
{
    public class CourseService : ICourseService
    {
        private IDataHandler data;

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

        public Course RetrieveCourse(string coursename)
        {
            var course = this.data.Courses.All()
                .Include(co => co.Teacher)
                .FirstOrDefault(co => co.Name == coursename);

            return course;
        }

    }
}
