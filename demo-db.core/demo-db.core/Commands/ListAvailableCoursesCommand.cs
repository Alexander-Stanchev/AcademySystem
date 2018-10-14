using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    public class ListAvailableCoursesCommand : CommandAbstract
    {
        private ICourseService serviceCourse;

        public ListAvailableCoursesCommand(ISessionState state, IStringBuilderWrapper builder, IUserService serviceUser, ICourseService serviceCourse) : base(state, builder)
        {
            this.serviceCourse = serviceCourse;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("Please log before using commands");
            }
            else if (this.State.RoleId == 1)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Student or Teacher");
            }
            else
            {
                var courses = this.serviceCourse.RetrieveCourseNames(this.State.RoleId, this.State.UserName);
                if (courses.Count == 0)
                {
                    return "There are no available courses at this moment!";
                }
                else
                {
                    this.Builder.AppendLine("The available courses are:");
                    foreach (var course in courses)
                    {
                        this.Builder.AppendLine($"Course: {course.CourseName} with teacher: {course.Teacher.FullName}");
                    }
                    return this.Builder.ToString();
                }
            }
        }
    }
}
