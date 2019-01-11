using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Test.Domain.Extend;
using Test.Domain.Service.Impl;
using Test.Domain.Service.Interface;

namespace Test.Domain.IOC
{
    /// <summary>
    /// DomainServiceModule
    /// </summary>
    public class DomainServiceModule: Module
    {
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommentDomainSvc>().As<ICommentDomainSvc>().InstancePerLifetimeScope();
        }
    }
}
