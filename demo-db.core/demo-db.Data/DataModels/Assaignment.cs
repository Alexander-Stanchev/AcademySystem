using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class Assaignment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int MaxPoints { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
