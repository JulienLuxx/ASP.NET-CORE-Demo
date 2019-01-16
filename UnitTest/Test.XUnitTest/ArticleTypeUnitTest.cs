using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
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
            var data=new ArticleType
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
        { }
    }
}
