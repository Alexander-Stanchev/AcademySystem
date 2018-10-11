using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Exceptions
{
    public class NotEnrolledInCourseException : Exception
    {
        public NotEnrolledInCourseException(string message) : base(message)
        {

        }
    }
}
