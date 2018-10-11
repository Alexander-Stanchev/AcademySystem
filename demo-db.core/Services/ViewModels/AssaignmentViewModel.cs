using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Services.ViewModels
{
    public class AssaignmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseViewModel Course { get; set; }
        public int MaxPoints { get; set; }
    }
}
