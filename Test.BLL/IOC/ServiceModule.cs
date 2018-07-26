using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Service.Impl;
using Test.Service.Interface;

namespace Test.Service.IOC
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticleSvc>().As<IArticleSvc>().InstancePerLifetimeScope();
            builder.RegisterType<CommentSvc>().As<ICommentSvc>().InstancePerLifetimeScope();
        }
    }
}
