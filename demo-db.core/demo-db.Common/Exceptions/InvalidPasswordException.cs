using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {

        }
    }
}
