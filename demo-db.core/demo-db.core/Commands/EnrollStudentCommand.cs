using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;

namespace demo_db.core.Commands
{
    public class EnrollStudentCommand : CommandAbstract
    {
        private readonly IUserService serviceUser;
        private readonly ICourseService serviceCourse;

        public EnrollStudentCommand(ISessionState state, IStringBuilderWrapper builder, IUserService service, ICourseService course) : base(state,builder)
        {
            this.serviceUser = service;
            this.serviceCourse = course;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("Please log before using commands");
            }
            else if(this.State.RoleId != 3)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Student");
            }
            else
            {
                var courseName = string.Join(' ',parameters);

                try
                {
                    this.serviceCourse.EnrollStudent(this.State.UserName, courseName);
                    return $"User {this.State.UserName} successfully enrolled in the course {courseName}";

                }
                catch(CourseAlreadyEnrolledException ex)
                {
                    return ex.Message;
                }
                catch(CourseDoesntExistsException ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
