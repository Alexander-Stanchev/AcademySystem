using demo_db.Common.Exceptions;
using demo_db.core.Contracts;
using demo_db.core.Export;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    public class ExportGradesCommand : CommandAbstract
    {
        private readonly ICourseService serviceCourse;

        public ExportGradesCommand(ISessionState state, ICourseService service) : base(state)
        {
            this.serviceCourse = service;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                throw new UserNotLoggedException("Please log before using commands");
            }
            else if (this.State.RoleId != 3)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Student");
            }
            else
            {
                var grades = this.serviceCourse.RetrieveGrades(this.State.UserName);                
                
                ExportToPDF.GeneratePDFReport(grades, this.State.UserName);
                return "Created PDF Report";
            }
            
        }
        
    }
}
