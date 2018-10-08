using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class EnrolledStudent
    {

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }
    }
}
