using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Test.Service.Interface;
using Xunit;

namespace Test.XUnitTest
{
    public class TestUnitTest
    {
        Mock<DbSet<ArticleType>> _mockSet { get; set; }
        Mock<TestDBContext> _mockContext { get; set; }
        IArticleTypeSvc _mockSvc { get; set; }
        public TestUnitTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(typeof(CustomizeProfile));
            var services = serviceCollection.BuildServiceProvider();
            var mapper = services.GetService<IMapper>();

            //Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            _mockSet = new Mock<DbSet<ArticleType>>();
            _mockContext = new Mock<TestDBContext>();
            _mockContext.Setup(x => x.ArticleType).Returns(_mockSet.Object);
            _mockSvc = new ArticleTypeSvc(mapper,_mockContext.Object, new DbContextExtendSvc());
        }

        [Fact]
        public void AddSingleTest()
        {
            var data = new ArticleTypeDto() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            _mockSvc.AddSingle(data);

            _mockContext.Verify(x => x.Add(It.IsAny<ArticleType>()), Times.Once());
            _mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }
    }
}
