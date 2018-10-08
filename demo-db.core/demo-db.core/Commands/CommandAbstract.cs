using demo_db.core.Contracts;

namespace demo_db.core.Commands
{
    public abstract class CommandAbstract : ICommand
    {
        public CommandAbstract(ISessionState state)
        {
            this.State = state;
        }
        public ISessionState State { get; private set; }

        public abstract string Execute(string[] parameters);      
    }
}
