﻿using demo_db.Common.Enum;
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
                else if (string.IsNullOrEmpty(parameters[0]))
                {
                    throw new ArgumentNullException("Username is null");
                }
                else if (string.IsNullOrEmpty(parameters[1]))
                {
                    throw new ArgumentNullException("Password is null");
                }
                var userName = parameters[0];
                var password = parameters[1];
                try
                {
                    var role = this.service.LoginUser(userName, password);
                    this.State.Login(role,userName);
                    return $"User {userName} succesfully logged. Your role is {(RoleEnum)(role-1)}";
                }
                catch(EntityAlreadyExistsException ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
