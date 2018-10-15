using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;


namespace demo_db.core.Commands
{
    class UpdateStudentToTeacherCommand : CommandAbstract
    {
        private IUserService service;
        private const int teacherRoleId = 2;

        public UpdateStudentToTeacherCommand(ISessionState state, IStringBuilderWrapper builder, IUserService service) : base(state, builder)
        {
            this.service = service;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("You have to log in first.");
            }

            if (this.State.RoleId != 1)
            {
                throw new IncorrectPermissionsException("You dont have access.");
            }

            if (parameters[0] == null)
            {
                throw new ArgumentNullException("userName is null");
            }

            string userName = parameters[0];
            
            if (parameters.Length > 1)
            {
                throw new ArgumentOutOfRangeException("You are passing more parameters than needed. You need to specify just the username");
            }

            this.service.UpdateRole(userName, teacherRoleId);

            return $"User {userName} role is updated.";
        }
    }
}