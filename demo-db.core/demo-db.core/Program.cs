using Autofac;
using demo_db.core.Contracts;
using demo_db.core.InjectionLogic;
using System;
using System.Reflection;

namespace demo_db.core
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetAssembly(typeof(InjectorModule)));

            var container = builder.Build();

            var engine = container.Resolve<IEngine>();
            engine.Run();
        }
    }
}
