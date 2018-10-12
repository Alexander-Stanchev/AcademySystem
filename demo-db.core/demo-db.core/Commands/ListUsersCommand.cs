using System;
using System.Collections.Generic;
using System.Text;
using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;

namespace demo_db.core.Commands
{
    class ListUsersCommand : CommandAbstract
    {
        private readonly IUserService serviceUser;

        public ListUsersCommand(ISessionState state, IStringBuilderWrapper builder, IUserService serviceUser) : base(state, builder)
        {
            this.serviceUser = serviceUser;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("Please log before using commands");
            }
            else if (this.State.RoleId != 1)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Admin");
            }
            else
            {
                var roleId = int.Parse(parameters[0]);
                var users = this.serviceUser.RetrieveUsers(roleId);
                if (users.Count == 0)
                {
                    return "There are no users.";
                }
                else
                {
                    this.Builder.AppendLine("The registered users are:");
                    foreach (var user in users)
                    {
                        this.Builder.AppendLine($"User {user.FullName} with username {user.Username}");
                    }
                    return this.Builder.ToString();
                }                
            }
        }
    }
}
