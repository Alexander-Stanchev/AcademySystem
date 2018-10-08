using demo_db.core.Contracts;
using demo_db.Services.Abstract;



namespace demo_db.core.Commands
{
    public class LogInCommand : CommandAbstract, ICommand
    {
        private IUserService service;

        public LogInCommand(ISessionState state, IUserService service) : base(state)
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
                var user = service.RetrieveUser(userName);
                if(user == null)
                {
                    return "User doesn't exist";
                }
                else if(user.Password == password)
                {
                    this.State.Login(user.RoleId, user.UserName);
                    return $"User {user.UserName} succesfully logged. Your role is {user.Role.Name}";
                }
                else
                {
                    return "Invalid password";
                }

            }
        }
    }
}
