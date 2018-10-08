using demo_db.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface ICommand
    {
        string Execute(string[] parameters);
    }
}
