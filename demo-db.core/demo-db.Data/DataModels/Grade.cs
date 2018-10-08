using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class Grade
    {
        public int AssaignmentId { get; set; }
        public Assaignment Assaignment{ get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public double ReceivedGrade { get; set; }
    }
}
