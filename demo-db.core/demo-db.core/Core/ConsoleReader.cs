using demo_db.core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Core
{
    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
