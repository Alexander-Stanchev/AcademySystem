using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface IWriter
    {
        void Write(string message);

        void WriteLine(string message);
    }
}
