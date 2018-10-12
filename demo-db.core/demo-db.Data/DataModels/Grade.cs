using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class Grade
    {
        public int AssaignmentId { get; set; }
        public Assaignment Assaignment{ get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        [Range(0,100)]
        public double ReceivedGrade { get; set; }
    }
}
