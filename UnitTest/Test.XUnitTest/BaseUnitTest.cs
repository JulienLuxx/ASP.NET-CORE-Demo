using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Service.Infrastructure;

namespace Test.XUnitTest
{
    public class BaseUnitTest
    {
        protected ServiceCollection _serviceCollection { get; set; }

        protected ServiceProvider _serviceProvider { get; set; }

        protected BaseUnitTest()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddAutoMapper(typeof(CustomizeProfile));
        }

        protected void BuilderServiceProvider()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
