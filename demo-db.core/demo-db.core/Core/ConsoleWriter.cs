using demo_db.core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Core
{
    public class ConsoleWriter : IWriter
    {
        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
