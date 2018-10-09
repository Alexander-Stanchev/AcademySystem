using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Exceptions
{
    public class UserNotLoggedException : Exception
    {
        public UserNotLoggedException(string message) : base(message)
        {

        }
    }
}
