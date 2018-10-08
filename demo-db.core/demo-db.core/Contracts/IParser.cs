using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface IParser
    {
        ICommand ParseCommand(string commandName);
    }
}
