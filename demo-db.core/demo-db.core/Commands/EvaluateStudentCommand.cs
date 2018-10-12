using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    public class EvaluateStudentCommand : CommandAbstract
    {
        private IUserService serviceUser;

        public EvaluateStudentCommand(ISessionState state, IStringBuilderWrapper builder, IUserService serviceUser) : base(state, builder)
        {
            this.serviceUser = serviceUser;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("Please log before using commands");
            }
            else if (this.State.RoleId != 2)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Teacher");
            }
            else
            {
                string username = parameters[0];
                int assignmentId = int.Parse(parameters[1]);
                int grade = int.Parse(parameters[2]);

                this.serviceUser.EvaluateStudent(username, assignmentId, grade, this.State.UserName);

                return "Grade successfully added.";
            }


        }
    }
}

