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
                else if (string.IsNullOrEmpty(parameters[0]))
                {
                    throw new ArgumentNullException("Username is null");
                }
                else if (string.IsNullOrEmpty(parameters[1]))
                {
                    throw new ArgumentNullException("Password is null");
                }
                else if (parameters[2] == null)
                {
                    throw new ArgumentNullException("Full name is null");
                }
                
                var userName = parameters[0];
                var password = parameters[1];
                var fullName = string.Join(' ',parameters.Skip(2));
                try
                {
                    this.service.AddUser(userName, password, fullName);
                    return $"User {userName} is registered. Now you can log in with your newly created password";
                }
                catch(EntityAlreadyExistsException ex)
                {                    
                    return ex.Message;
                }
            }
        }
    }
}
