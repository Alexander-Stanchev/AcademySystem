using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface IProcessor
    {
        string ProcessCommand(string line);
    }
}
