using System;
using System.Collections.Generic;
using System.Text;
using demo_db.core.Contracts;
using demo_db.Data.DataModels;
using demo_db.Services;
using demo_db.Services.Abstract;

namespace demo_db.core.Commands
{
    class UpdateUserRoleCommand : CommandAbstract
    {
        private IUserService service;
        private IRoleService roleService;

        public UpdateUserRoleCommand(ISessionState state, IUserService service, IRoleService roleService) : base(state)
        {
            this.service = service;
            this.roleService = roleService;
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

            var user = service.RetrieveFullUser(userName);
            var role = new Role();
            if (user == null)
            {
                return "User doesn't exist";
            }
            else
            {
                role = this.roleService.RetrieveRole(newRoleString);

                user.RoleId = role.Id;
            }

            service.UpdateUserRole(userName, role);

            return $"User {userName} role is updated.";
        }
    }
}
