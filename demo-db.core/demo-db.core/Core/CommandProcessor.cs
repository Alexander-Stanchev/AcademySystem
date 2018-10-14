using demo_db.core.Contracts;
using System;
using System.Linq;


namespace demo_db.core.Core
{
    public class CommandProcessor : IProcessor
    {
        private readonly IParser parser;

        public CommandProcessor(IParser parser)
        {
            this.parser = parser;
        }

        public string ProcessCommand(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentNullException("Command cannot be null or empty.");
            }
            var arguments = line.Split(" ").ToList();
            var commandName = arguments[0].ToLower();
            var commandArguments = arguments.Skip(1).ToArray();
            var command = parser.ParseCommand(commandName);
            if (command == null)
            {
                throw new ArgumentNullException("Command not found");
            }
            else
            {
                return command.Execute(commandArguments);
            }
        }
    }
}
