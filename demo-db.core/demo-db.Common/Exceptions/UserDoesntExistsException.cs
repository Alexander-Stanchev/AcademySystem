using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Exceptions
{
    public class UserDoesntExistsException : Exception
    {
        public UserDoesntExistsException(string message) : base(message)
        {

        }
    }
}
