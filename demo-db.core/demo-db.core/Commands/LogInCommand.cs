using demo_db.Common.Enum;
using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;

namespace demo_db.core.Commands
{
    public class LogInCommand : CommandAbstract
    {
        private IUserService service;

        public LogInCommand(ISessionState state, IStringBuilderWrapper builder, IUserService service) : base(state,builder)
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
                if (parameters.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("You should provide at least 2 parameters for this command");
                }
                else if (parameters[0] == null)
                {
                    throw new ArgumentNullException("User name is null");
                }
                else if (parameters[1] == null)
                {
                    throw new ArgumentNullException("Password is null");
                }
                string userName = parameters[0];
                string password = parameters[1];
                try
                {
                    var role = this.service.LoginUser(userName, password);
                    this.State.Login(role,userName);
                    return $"User {userName} succesfully logged. Your role is {(RoleEnum)(role-1)}";
                }
                catch(UserAlreadyExistsException ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
