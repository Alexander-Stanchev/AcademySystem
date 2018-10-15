using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;


namespace demo_db.core.Commands
{
    public class UpdateStudentToTeacherCommand : CommandAbstract
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
                return("You have to log in first.");
            }

            if (this.State.RoleId != 1)
            {
                return("You dont have access.");
            }

            if (string.IsNullOrEmpty(parameters[0]))
            {
                throw new ArgumentNullException("userName is null");
            }

            string userName = parameters[0];
            
            if (parameters.Length > 1)
            {
                throw new ArgumentOutOfRangeException("You are passing more parameters than needed. You need to specify just the username");
            }
            try
            {
                this.service.UpdateRole(userName, teacherRoleId);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            catch (ArgumentNullException ex)
            {
                return ex.Message;
            }


            return $"User {userName} role is updated.";
        }
    }
}