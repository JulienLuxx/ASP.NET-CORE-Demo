using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Test.Service.Interface;
using Test.Service.QueryModel;
using Test.XUnitTest.Infrastructure;
using Xunit;

namespace Test.XUnitTest
{
    public class ArticleTypeUnitTest
    {
        #region VerifyMethod
        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public void GetPageDataTest()
        {
            var dbMock = new Mock<TestDBContext>();
            var svcMock = new ArticleTypeSvc(dbMock.Object, new DbContextExtendSvc());
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=1,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var articleTypeSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            dbMock.Setup(x => x.ArticleType).Returns(articleTypeSet.Object);
            var data = svcMock.GetPageData(new ArticleTypeQueryModel());
            Assert.Equal(2, data.List.Count());
        }

        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public async Task GetPageDataAsyncTest()
        {
            var mockContext = new Mock<TestDBContext>();
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=2,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var mockSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var result = await mockSvc.GetPageDataAsync(new ArticleTypeQueryModel());
            Assert.Equal(2, result.List.Count());
        }

        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public async Task GetSingleDataAsyncTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockContext = new Mock<TestDBContext>();
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=2,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var mockSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var result = await mockSvc.GetSingleDataAsync(1);
            Assert.Equal(1, result.Data.Id);
        }

        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public void AddSingleTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = new Mock<DbSet<ArticleType>>();
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);

            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var dto = new ArticleTypeDto() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            mockSvc.AddSingle(dto);

            mockContext.Verify(x => x.Add(It.IsAny<ArticleType>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public void EditTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=2,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var mockSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            var dto = new ArticleTypeDto() { Id = 1, Name = "666", EditerName = "test", CreateTime = DateTime.Now };
            mockSvc.Edit(dto);

            mockContext.Verify(x => x.Update(It.IsAny<ArticleType>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// VerifyMethod
        /// </summary>
        [Fact]
        public void DeleteTest()
        {
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=2,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var mockSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var mockSvc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
            mockSvc.Delete("1,2");

            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            var result1 = mockSet.Object.Where(x => x.Id == 1).Select(s => s.IsDeleted).FirstOrDefault();
            var result2 = mockSet.Object.Where(x => x.Id == 2).Select(s => s.IsDeleted).FirstOrDefault();
            Assert.True(result1);
            Assert.True(result2);
        }
        #endregion

        #region UnVerify&FailedMethod
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
            var mockSvc = new ArticleTypeSvc(db, new DbContextExtendSvc());
            var qModel = new ArticleTypeQueryModel();
            var result = mockSvc.GetPageDataAsync(qModel).GetAwaiter().GetResult();
            Assert.NotNull(result.List);
        }

        [Fact]
        public void AddDataTest()
        {
            var mockSet = new Mock<DbSet<Article>>();
            var mockContext = new Mock<TestDBContext>();
            var data = new ArticleType() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            var svc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
        }

        [Fact]
        public void GetDataTest()
        {
            var query = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=1,Name="2",EditerName="223",CreateTime=DateTime.Now},
            }.AsQueryable();

            //var mockSet = new Mock<DbSet<ArticleType>>();
            //mockSet.As<IQueryable<ArticleType>>

            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType.AsNoTracking()).Returns(query);

            var svc = new ArticleTypeSvc(mockContext.Object, new DbContextExtendSvc());
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

            var svc = new ArticleTypeSvc(mockContext, new DbContextExtendSvc());
            svc.Add(new ArticleTypeDto() { Name = "UnitTest", EditerName = "admin" });

            mockSet.Received(1).Add(Arg.Any<ArticleType>());
            mockContext.Received(1).SaveChanges();

            addArticleType.Name.ShouldBe("UnitTest");
            addArticleType.EditerName.ShouldBe("admin");
        }
        #endregion               
    }
}
