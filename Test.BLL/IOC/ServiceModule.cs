using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Service.Impl;
using Test.Service.Interface;

namespace Test.Service.IOC
{
    /// <summary>
    /// ServiceModule
    /// </summary>
    public class ServiceModule: Module
    {
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArticleSvc>().As<IArticleSvc>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleTypeSvc>().As<IArticleTypeSvc>().InstancePerLifetimeScope();
            builder.RegisterType<CommentSvc>().As<ICommentSvc>().InstancePerLifetimeScope();
            builder.RegisterType<UserSvc>().As<IUserSvc>().InstancePerLifetimeScope();
            builder.RegisterType<LogSvc>().As<ILogSvc>().InstancePerLifetimeScope();
        }
    }
}
