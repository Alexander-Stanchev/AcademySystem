using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int TeacherId { get; set; }
        public UserViewModel Teacher { get; set; }
        public ICollection<UserViewModel> EnrolledStudents { get; set; }
    }
}
