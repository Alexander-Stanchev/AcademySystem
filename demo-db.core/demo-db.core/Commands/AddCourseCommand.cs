﻿using demo_db.core.Contracts;
using demo_db.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Commands
{
    class AddCourseCommand : CommandAbstract
    {
        private ICourseService service;

        public AddCourseCommand(ISessionState state, ICourseService service) : base(state)
        {
            this.service = service;
        }
        public override string Execute(string[] parameters)
        {
            if (!this.State.IsLogged)
            {
                return "You will need to login first";
            }
            else
            {
                string course = parameters[0];
                DateTime start = DateTime.Parse(parameters[1]);
                DateTime end = DateTime.Parse(parameters[2]);


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