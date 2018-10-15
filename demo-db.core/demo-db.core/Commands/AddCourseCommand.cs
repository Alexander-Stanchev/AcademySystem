using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;

namespace demo_db.core.Commands
{
    class AddCourseCommand : CommandAbstract
    {
        private ICourseService service;

        public AddCourseCommand(ISessionState state, IStringBuilderWrapper builder, ICourseService service) : base(state, builder)
        {
            this.service = service;
        }
        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("You will need to login first");
            }
            else if (this.State.RoleId != 2)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Teacher");
            }
            else
            {
                string course = parameters[0].Replace('_', ' ');
                DateTime start;
                DateTime end;             

                if (course == string.Empty)
                {
                    throw new Exception("The course name can`t be null");
                }

                try
                {
                    start = DateTime.Parse(parameters[1]);
                    end = DateTime.Parse(parameters[2]);
                }
                catch (Exception)
                {
                    throw new Exception("Please enter valid DateTime ");
                }

                try
                {
                    this.service.AddCourse(course, this.State.UserName, start, end);
                    return $"Course {course} is registered.";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
