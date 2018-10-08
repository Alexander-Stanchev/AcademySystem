﻿using demo_db.core.Common;
using demo_db.core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.core.Core
{
    public class Engine : IEngine
    {
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
               this.Writer.WriteLine("For registration use the following command: Register {username} {password}");
               var input = this.Reader.ReadLine();
                try
                {
                    var execution = this.Processor.ProcessCommand(input);
                    this.Writer.WriteLine(execution);
                }
                catch(ArgumentNullException ex)
                {
                    this.Writer.WriteLine(ex.Message);
                }

            }
            this.Writer.WriteLine($"Logged user: {this.State.UserName} with role: {(RoleEnum)(this.State.RoleId-1)}");
        }
    }
}