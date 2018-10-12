using demo_db.Common.Enum;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;

namespace demo_db.core.Commands
{
    public class LogInCommand : CommandAbstract
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
                try
                {
                    var role = this.service.LoginUser(userName, password);
                    this.State.Login(role,userName);
                    return $"User {userName} succesfully logged. Your role is {(RoleEnum)(role-1)}";
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
