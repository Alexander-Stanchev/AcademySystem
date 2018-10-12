using Autofac;
using demo_db.core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Core
{
    public class CommandParser : IParser
    {
        private readonly ILifetimeScope scope;

        public CommandParser(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public ICommand ParseCommand(string commandName)
        {
            try
            {
                return scope.ResolveNamed<ICommand>(commandName.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
