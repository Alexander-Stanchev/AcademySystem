using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.ViewModels
{
    public class GradeViewModel
    {
        public AssaignmentViewModel Assaingment { get; set; }
        public UserViewModel Student { get; set; }
        public double Score { get; set; }
    }
}
