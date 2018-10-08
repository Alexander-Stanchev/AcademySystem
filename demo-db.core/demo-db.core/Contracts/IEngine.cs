using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Contracts
{
    public interface IEngine
    {
        IReader Reader { get; }

        IWriter Writer { get; }

        IProcessor Processor { get; }

        void Run();
    }
}
