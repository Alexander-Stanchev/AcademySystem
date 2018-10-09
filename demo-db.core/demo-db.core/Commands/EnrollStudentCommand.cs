using demo_db.core.Contracts;
using demo_db.core.Exceptions;
using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Linq;


namespace demo_db.core.Commands
{
    public class EnrollStudentCommand : CommandAbstract
    {
        private IUserService serviceUser;
        private ICourseService serviceCourse;

        public EnrollStudentCommand(ISessionState state, IUserService service, ICourseService course) : base(state)
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
                var user = this.serviceUser.RetrieveFullUser(this.State.UserName);
                var courseName = string.Join(' ',parameters);

                var course = this.serviceCourse.RetrieveCourse(courseName);
                if(course == null)
                {
                    return $"Unfortunately such a course dosn't exist!";
                }
                try
                {
                    this.serviceUser.EnrollCourse(user, course);
                }
                catch(Exception)
                {
                    throw new CourseAlreadyEnrolledException("You are already enrolled for this course");
                }
                return $"User {user.UserName} succesfully enrolled in the course {course.Name}";


            }
        }
    }
}
