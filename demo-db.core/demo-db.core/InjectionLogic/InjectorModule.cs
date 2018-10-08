using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using System.Linq;
using demo_db.core.Contracts;
using demo_db.core.Core;
using demo_db.Data.Context;
using demo_db.Services;
using demo_db.Services.Abstract;

namespace demo_db.core.InjectionLogic
{
    public class InjectorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            this.RegisterCommands(builder);
            this.RegisterCoreComponents(builder);
            this.RegisterServices(builder);
            base.Load(builder);
        }
        private void RegisterCoreComponents(ContainerBuilder builder)
        {
            builder.RegisterType<Engine>().As<IEngine>().SingleInstance();
            builder.RegisterType<ConsoleReader>().As<IReader>().SingleInstance();
            builder.RegisterType<ConsoleWriter>().As<IWriter>().SingleInstance();
            builder.RegisterType<CommandParser>().As<IParser>().SingleInstance();
            builder.RegisterType<CommandProcessor>().As<IProcessor>().SingleInstance();
            builder.RegisterType<SessionState>().As<ISessionState>().SingleInstance();
            builder.RegisterType<AcademyContext>().As<IAcademyContext>();
        }
        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
        }
        private void RegisterCommands(ContainerBuilder builder)
        {
            var assembly = Assembly.Load("demo-db.core");
            var types = assembly.DefinedTypes
                .Where(t => t.ImplementedInterfaces.Any(inter => inter == typeof(ICommand)))
                .Where(type => type.Name.ToLower().EndsWith("command"));

            foreach(var command in types)
            {
                builder.RegisterType(command.UnderlyingSystemType)
                    .Named<ICommand>(command.Name.ToLower().Replace("command", string.Empty));
            }
        }
    }
}
