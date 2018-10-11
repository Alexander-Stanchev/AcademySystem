using System;

namespace demo_db.Common.Exceptions
{
    public class UserNotLoggedException : Exception
    {
        public UserNotLoggedException(string message) : base(message)
        {

        }
    }
}
