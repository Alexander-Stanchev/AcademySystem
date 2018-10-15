using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;

namespace demo_db.core.Commands
{
    public class CheckGradesForCourseCommand : CommandAbstract
    {
        private readonly ICourseService serviceCourse;
        
        public CheckGradesForCourseCommand(ISessionState state, IStringBuilderWrapper builder, ICourseService service) : base(state, builder)
        {
            this.serviceCourse = service;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return("You will need to login first");
            }
            else if (this.State.RoleId != 3)
            {
                return("This command is available only to users with role Student");
            }
            else
            {
                var coursename = string.Join(' ', parameters);

                if (coursename == string.Empty)
                {
                    return "The course name can`t be null";
                }

                try
                {
                    var grades = this.serviceCourse.RetrieveGrades(this.State.UserName, coursename);
                    if(grades.Count == 0)
                    {
                        return $"No grades available for the course {coursename}";
                    }

                    this.Builder.AppendLine($"Grades for {coursename}");
                    foreach (var grade in grades)
                    {
                        Builder.AppendLine($"{grade.Assaingment.Name}: {grade.Score} out of {grade.Assaingment.MaxPoints}");
                    }
                    return Builder.ToString();
                }
                catch(NotEnrolledInCourseException ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
