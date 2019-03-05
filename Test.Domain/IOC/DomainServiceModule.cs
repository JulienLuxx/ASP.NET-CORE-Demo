using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain.Extend;

namespace Test.Domain.IOC
{
    public class DomainServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContextExtendSvc>().As<IDbContextExtendSvc>().InstancePerLifetimeScope();
        }
    }
}
