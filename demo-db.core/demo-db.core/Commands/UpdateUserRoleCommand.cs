using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    class UpdateUserRoleCommand : CommandAbstract, ICommand
    {
        private IUserService service;

        public UpdateUserRoleCommand(ISessionState state, IUserService service) : base(state)
        {
            this.service = service;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return "You have to log in first.";
            }

            if (this.State.RoleId != 1)
            {
                return "You dont have access.";
            }

            string userName = parameters[0];
            string newRoleString = parameters[1];

            this.service.UpdateRole(userName, newRoleString);

            return $"User {userName} role is updated.";
        }
    }
}

