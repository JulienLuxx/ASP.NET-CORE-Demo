using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Encrypt;
using Test.Core.Tree;
using Test.Core.Configuration;

namespace Test.Core.IOC
{
    public class UtilModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationGeter>().As<IConfigurationGeter>().InstancePerLifetimeScope();
            builder.RegisterType<TreeUtil>().As<ITreeUtil>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptUtil>().As<IEncryptUtil>().InstancePerLifetimeScope();
        }
    }
}
