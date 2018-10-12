using demo_db.Common.Wrappers;
using demo_db.core.Contracts;

namespace demo_db.core.Commands
{
    public abstract class CommandAbstract : ICommand
    {
        public CommandAbstract(ISessionState state, IStringBuilderWrapper builder)
        {
            this.State = state;
            this.Builder = builder;
        }
        public ISessionState State { get; private set; }
        public IStringBuilderWrapper Builder { get; set; }
        public abstract string Execute(string[] parameters);      
    }
}
