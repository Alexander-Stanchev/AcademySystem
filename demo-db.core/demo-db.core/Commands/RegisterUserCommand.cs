using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System.Linq;
using System;


namespace demo_db.core.Commands
{
    public class RegisterUserCommand : CommandAbstract
    {
        private IUserService service;

        public RegisterUserCommand(ISessionState state, IStringBuilderWrapper builder, IUserService service) : base(state,builder)
        {
            this.service = service;
        }
        public override string Execute(string[] parameters)
        {
            if (this.State.IsLogged)
            {
                return("You are already logged.");
            }
            else
            {
                if (parameters.Length < 4)
                {
                    throw new ArgumentOutOfRangeException("You should provide at least 4 parameters for this command");
                }
                else if (parameters[0] == null)
                {
                    throw new ArgumentNullException("User name is null");
                }
                else if (parameters[1] == null)
                {
                    throw new ArgumentNullException("Password is null");
                }
                else if (parameters[2] == null)
                {
                    throw new ArgumentNullException("Full name is null");
                }
                
                string userName = parameters[0];
                string password = parameters[1];
                string fullName = string.Join(' ',parameters.Skip(2));
                try
                {
                    this.service.AddUser(userName, password, fullName);
                    return $"User {userName} is registered. Now you can log in with your newly created password";
                }
                catch(UserAlreadyExistsException ex)
                {                    
                    return ex.Message;
                }
            }
        }
    }
}
