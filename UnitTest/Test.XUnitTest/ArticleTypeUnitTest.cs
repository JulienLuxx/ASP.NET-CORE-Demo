using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Test.Service.Interface;
using Test.Service.QueryModel;
using Xunit;

namespace Test.XUnitTest
{
    public class ArticleTypeUnitTest
    {
        [Fact]
        public void Test1()
        {
            var articleType = new ArticleType
            {
                Name = "Test",
                EditerName = "admin",
            };
            Assert.Contains("Test", articleType.Name);
        }

        [Fact]
        public void Test2()
        {
            var data = new ArticleType
            {
                Name = "Test",
                EditerName = "admin",
            };
            Assert.Equal("admin", data.EditerName);
        }

        [Fact]
        public void Test3()
        {
            var data = new ArticleType
            {
                Name = "Test",
                EditerName = "admin",
            };
            Assert.IsType<ArticleType>(data);
        }

        [Fact]
        public void GetListTest()
        {
            var db = new TestDBContext();
            var mockSvc = new ArticleTypeSvc(db);
            var qModel = new ArticleTypeQueryModel();
            var result = mockSvc.GetPageDataAsync(qModel).GetAwaiter().GetResult();
            Assert.NotNull(result.List);
        }

        [Fact]
        public void AddDataTest()
        {
            var mockdb = new Mock<TestDBContext>();
            var data = new ArticleType() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            var svc = new ArticleTypeSvc(mockdb.Object);
        }

        [Fact]
        public void GetDataTest()
        {
            var query = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=1,Name="2",EditerName="223",CreateTime=DateTime.Now},
            }.AsQueryable();

            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.Set<ArticleType>().AsQueryable()).Returns(query);

            var svc = new ArticleTypeSvc(mockContext.Object);
            var result = svc.GetPageData(new ArticleTypeQueryModel());
            Assert.Equal(2, result.List.Count());
        }


        [Fact]
        public void AddTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = Substitute.For<DbSet<ArticleType>>();
            var mockContext = Substitute.For<TestDBContext>();
            mockContext.ArticleType.Returns(mockSet);

            var addArticleType = new ArticleType();
            mockSet.Add(Arg.Do<ArticleType>(x => addArticleType = x));

            var svc = new ArticleTypeSvc(mockContext);
            svc.Add(new ArticleTypeDto() { Name = "UnitTest", EditerName = "admin" });

            mockSet.Received(1).Add(Arg.Any<ArticleType>());
            mockContext.Received(1).SaveChanges();

            addArticleType.Name.ShouldBe("UnitTest");
            addArticleType.EditerName.ShouldBe("admin");
        }

        public void GetPageTest()
        {
            var dataList = new List<ArticleType>()
            {
                new ArticleType()
                {
                    Id=1,
                    Name="1",
                    EditerName="admin",
                    CreateTime=DateTime.Now
                },
                new ArticleType()
                {
                    Id=2,
                    Name="2",
                    EditerName="admin",
                    CreateTime=DateTime.Now
                }
            };
            var mockSet = Substitute.For<DbSet<Article>, IQueryable<ArticleType>>();
        }
    }
}
