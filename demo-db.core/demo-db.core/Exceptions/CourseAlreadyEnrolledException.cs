using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Exceptions
{
    public class CourseAlreadyEnrolledException : Exception
    {
        public CourseAlreadyEnrolledException(string message) : base(message)
        {

        }
    }
}
