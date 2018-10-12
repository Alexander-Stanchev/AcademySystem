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
        [MaxLength(35)]
        public string UserName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(40)]
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
