using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System.Linq;


namespace demo_db.core.Commands
{
    public class RegisterUserCommand : CommandAbstract, ICommand
    {
        private IUserService service;

        public RegisterUserCommand(ISessionState state, IUserService service) : base(state)
        {
            this.service = service;
        }
        public override string Execute(string[] parameters)
        {
            if (this.State.IsLogged)
            {
                return "You are already logged.";
            }
            else
            {
                string userName = parameters[0];
                string password = parameters[1];
                string fullName = string.Join(' ',parameters.Skip(2));
                var user = service.RetrieveUser(userName);
                if (user != null)
                {
                    return "User already exist";
                }
                else
                {
                    service.AddUser(userName, password, fullName);
                    return $"User {userName} is registered. Now you can log in with your newly created password";
                }

            }
        }
    }
}
