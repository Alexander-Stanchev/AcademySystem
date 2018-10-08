using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public int? MentorId { get; set; }

        public User Mentor { get; set; }

        public int RoleId { get; set; }

        public Role Role {get;set;}

        public bool Deleted { get; set; }

        public DateTime RegisteredOn { get; set; }

        public ICollection<Course> TaughtCourses { get; set; }

        public ICollection<EnrolledStudent> EnrolledStudents{ get; set; }

        public ICollection<Grade> Grades { get; set; }

    }
}
