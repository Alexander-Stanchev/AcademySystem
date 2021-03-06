﻿using demo_db.Common.Enum;
using demo_db.Common.Exceptions;
using demo_db.core.Contracts;
using System;

namespace demo_db.core.Core
{
    public class Engine : IEngine
    {
        private const string endCommand = "end";
        public Engine(IReader reader, IWriter writer, IProcessor processor, ISessionState state)
        {
            this.Reader = reader;
            this.Writer = writer;
            this.Processor = processor;
            this.State = state;
        }
        public IReader Reader { get; private set; }

        public IWriter Writer { get; private set; }

        public IProcessor Processor { get; private set; }

        public ISessionState State { get; private set; }

        public void Run()
        {
            while (!this.State.IsLogged)
            {
               this.Writer.WriteLine("Please login or register");
               this.Writer.WriteLine("For login use the following command: Login {username} {password}");
               this.Writer.WriteLine("For registration use the following command: RegisterUser {username} {password} {full name}");
               var input = this.Reader.ReadLine();
                try
                {
                    var execution = this.Processor.ProcessCommand(input);
                    this.Writer.WriteLine(execution);
                }
                catch(Exception ex)
                {
                    this.Writer.WriteLine(ex.Message);
                }

            }
            this.Writer.WriteLine($"Logged user: {this.State.UserName} with role: {(RoleEnum)(this.State.RoleId-1)}");

            if ((RoleEnum)(this.State.RoleId - 1) == RoleEnum.Administrator)
            {
                this.Writer.WriteLine("For changing the role of existing user use the following command: UpdateUserRole {username} {newRole}");
            }

            if ((RoleEnum)(this.State.RoleId - 1) == RoleEnum.Teacher)
            {
                this.Writer.WriteLine("For adding new course: AddCourse {dd-mm-yy} {dd-mm-yy} {course name}");
                this.Writer.WriteLine("For listing all courses you are assigned to: ListAvailableCourses");
                this.Writer.WriteLine("For listing all students in a course: ListStudents {course name}");
            }

            if ((RoleEnum)(this.State.RoleId - 1) == RoleEnum.Student)
            {
                this.Writer.WriteLine("For listing all courses you are assigned to: ListAvailableCourses");
            }

            while (true)
            {
                var command = this.Reader.ReadLine();
                if (command.ToLower() == Engine.endCommand.ToLower())
                {
                    break;
                }
                else
                {
                    try
                    {
                        var execution = this.Processor.ProcessCommand(command);
                        this.Writer.WriteLine(execution);
                    }
                    catch(Exception ex)
                    {
                        this.Writer.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
