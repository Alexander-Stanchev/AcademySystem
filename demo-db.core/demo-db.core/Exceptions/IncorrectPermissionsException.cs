using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Exceptions
{
    public class IncorrectPermissionsException : Exception
    {
        public IncorrectPermissionsException(string message) : base(message)
        {

        }
    }
}
