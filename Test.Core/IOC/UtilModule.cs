using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Encrypt;
using Test.Core.HttpUtl;
using Test.Core.Map;
using Test.Core.Tree;

namespace Test.Core.IOC
{
    /// <summary>
    /// Core.UtilInjection
    /// </summary>
    public class UtilModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TreeUtil>().As<ITreeUtil>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptUtil>().As<IEncryptUtil>().InstancePerLifetimeScope();
            builder.RegisterType<MapUtil>().As<IMapUtil>().InstancePerLifetimeScope();
            builder.RegisterType<HttpClientUtil>().As<IHttpClientUtil>().InstancePerLifetimeScope();
        }
    }
}
