using demo_db.Data.DataModels;
using demo_db.Services.ViewModels;
using System;
using System.Collections.Generic;

namespace demo_db.Services.Abstract
{
    public interface ICourseService
    {
        void AddCourse(string userName, DateTime start, DateTime end, string course);
        void EnrollStudent(string username, string coursename);
        IList<CourseViewModel> RetrieveCourseNames(int roleId, string username = "");
        IList<GradeViewModel> RetrieveGrades(string username, string coursename = "");
        IList<UserViewModel> RetrieveStudentsInCourse(string coursename, int roleId, string username);

    }
}
