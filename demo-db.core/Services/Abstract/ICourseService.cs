using demo_db.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.Abstract
{
    public interface ICourseService
    {
        void AddCourse(string coursename, int teacherID, DateTime start, DateTime end);
        Course RetrieveCourse(string coursename);

    }
}
