﻿using System;
using System.Collections.Generic;
using System.Text;
using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;

namespace demo_db.core.Commands
{
    public class ListUsersCommand : CommandAbstract
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
                return("Please log before using commands");
            }
            else if (this.State.RoleId != 1)
            {
                return("This command is available only to users with role Admin");
            }
            else
            {
                var roleId = 0;
                
                if (parameters.Length < 1)
                {
                    throw new ArgumentOutOfRangeException("You need to pass only one argument which is the role Id");
                }

                try
                {
                    roleId = int.Parse(parameters[0]);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                
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
