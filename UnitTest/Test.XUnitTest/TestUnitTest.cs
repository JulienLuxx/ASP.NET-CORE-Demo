using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoqEFCoreExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Xunit;

namespace Test.XUnitTest
{
    public class TestUnitTest
    {
        private List<ArticleType> _sampleList { get; set; }
        public TestUnitTest()
        {
            _sampleList= new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=1,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
        }

        [Fact]
        public void AddSingleTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = new Mock<DbSet<ArticleType>>();
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());

            var data = new ArticleTypeDto() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            mockSvc.AddSingle(data);

            mockContext.Verify(x => x.Add(It.IsAny<ArticleType>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public void GetPageDataTest()
        {
            var mockContext = new Mock<TestDBContext>();
            var mockSvc = new ArticleTypeSvc(mockContext.Object,new DbContextExtendSvc());
            var dataSet = new Mock<DbSet<ArticleType>>().SetupList(_sampleList);
            mockContext.Setup(x => x.ArticleType).Returns(dataSet.Object);
            var data = mockSvc.GetPageData(new Service.QueryModel.ArticleTypeQueryModel());
            Assert.Equal(_sampleList.Count(), data.List.Count());
        }

        [Fact]
        public async Task GetPageDataAsyncTest()
        {
            var mockContext = new Mock<TestDBContext>();
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var dataSet = new Mock<DbSet<ArticleType>>().SetupList(_sampleList);
            mockContext.Setup(x => x.ArticleType).Returns(dataSet.Object);
            var data = await mockSvc.GetPageDataAsync(new Service.QueryModel.ArticleTypeQueryModel());
            Assert.Equal(_sampleList.Count(), data.List.Count());
        }

        [Fact]
        public async Task GetSingleDataAsyncTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockContext = new Mock<TestDBContext>();
            var dataSet = new Mock<DbSet<ArticleType>>().SetupList(_sampleList);
            mockContext.Setup(x => x.ArticleType).Returns(dataSet.Object);
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var result = await mockSvc.GetSingleDataAsync(1);
            Assert.Equal(1, result.Data.Id);
        }
    }
}
