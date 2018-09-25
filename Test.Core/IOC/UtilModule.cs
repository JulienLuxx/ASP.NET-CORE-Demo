using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Encrypt;
using Test.Core.Tree;

namespace Test.Core.IOC
{
    public class UtilModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TreeUtil>().As<ITreeUtil>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptUtil>().As<IEncryptUtil>().InstancePerLifetimeScope();
        }
    }
}
