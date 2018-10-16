using System;

namespace demo_db.Common.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message)
        {

        }
    }
}
