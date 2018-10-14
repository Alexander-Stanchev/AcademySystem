using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Data.DataModels;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    public class ListStudentsCommand : CommandAbstract
    {
        private ICourseService serviceCourse;

        public ListStudentsCommand(ISessionState state, IStringBuilderWrapper builder, IUserService serviceUser, ICourseService serviceCourse) : base(state, builder)
        {
            this.serviceCourse = serviceCourse;
        }

        public override string Execute(string[] parameters)
        {
            string coursename = parameters[0].Replace('_', ' ');

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
                IList<User> students = this.serviceCourse.RetrieveStudentsInCourse(coursename, this.State.RoleId, this.State.UserName);

                if (students.Count == 0)
                {
                    return "There are no available students in this course!";
                }
                else
                {
                    this.Builder.AppendLine($"The available students in {coursename} are:");
                    foreach (var student in students)
                    {
                        var grades = this.serviceCourse.GradesString(student.UserName);

                        var gradesResult = grades.Count == 0 ? "None" : string.Join(", ", grades);
                    

                        this.Builder.AppendLine($"Username: {student.UserName}, full name: {student.FullName}, Grades: {gradesResult}");
                    }
                    return this.Builder.ToString();
                }
            }
        }
    }
}
