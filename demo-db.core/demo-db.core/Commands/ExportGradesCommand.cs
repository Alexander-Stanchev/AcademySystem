﻿using demo_db.Common.Exceptions;
using demo_db.Common.Wrappers;
using demo_db.core.Contracts;
using demo_db.core.Export.Abstract;
using demo_db.Services.Abstract;
using System;

namespace demo_db.core.Commands
{
    public class ExportGradesCommand : CommandAbstract
    {
        private readonly ICourseService serviceCourse;
        private readonly IExporter exporter;

        public ExportGradesCommand(ISessionState state, IStringBuilderWrapper builder, ICourseService service, IExporter exporter) : base(state, builder)
        {
            this.serviceCourse = service;
            this.exporter = exporter;
        }

        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return("Please log before using commands");
            }
            else if (this.State.RoleId != 3)
            {
                throw new IncorrectPermissionsException("This command is available only to users with role Student");
            }
            else
            {
                var grades = this.serviceCourse.RetrieveGrades(this.State.UserName);

                if (grades.Count == 0)
                {
                    return("There are no grades to export");
                }

                this.exporter.GenerateReport(grades, this.State.UserName);
                return "Created PDF Report";
            }            
        }        
    }
}
