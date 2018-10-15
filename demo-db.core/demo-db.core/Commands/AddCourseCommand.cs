using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                return("You will need to login first");
            }
            else if (this.State.RoleId != 2)
            {
                return("This command is available only to users with role Teacher");
            }
            else
            {
                string course = string.Join(' ', parameters.Skip(2)); ;
                DateTime start = DateTime.Parse(parameters[0]);
                DateTime end = DateTime.Parse(parameters[1]);

                try
                {
                    this.service.AddCourse(this.State.UserName, start, end, course);
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
