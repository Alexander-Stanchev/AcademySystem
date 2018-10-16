using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;

namespace demo_db.core.Commands
{
    public class EnrollStudentCommand : CommandAbstract
    {
        private readonly ICourseService serviceCourse;

        public EnrollStudentCommand(ISessionState state, IStringBuilderWrapper builder, ICourseService course) : base(state, builder)
        {
            this.serviceCourse = course;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return ("Please log before using commands");
            }
            if (this.State.RoleId != 3)
            {
                return ("This command is available only to users with role Student");
            }
            if (parameters.Length == 0)
            {
                return "Please enter a valid course name";
            }

            var courseName = string.Join(' ', parameters);

            try
            {
                this.serviceCourse.EnrollStudent(this.State.UserName, courseName);
                return $"User {this.State.UserName} successfully enrolled in the course {courseName}";

            }
            catch (CourseAlreadyEnrolledException ex)
            {
                return ex.Message;
            }
            catch (CourseDoesntExistsException ex)
            {
                return ex.Message;
            }
        }

    }
}
