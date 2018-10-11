using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
