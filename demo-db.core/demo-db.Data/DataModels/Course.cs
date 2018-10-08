using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        public int TeacherId { get; set; }

        public User Teacher { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ICollection<EnrolledStudent> EnrolledStudents { get; set; }
    }
}
