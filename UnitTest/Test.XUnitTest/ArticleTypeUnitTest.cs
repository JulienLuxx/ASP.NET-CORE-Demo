using Microsoft.EntityFrameworkCore;
using Moq;
using MoqEFCoreExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Impl;
using Xunit;

namespace Test.XUnitTest
{
    public class ArticleTypeUnitTest
    {
        [Fact]
        public void GetPageData()
        {
            var mockContext = new Mock<TestDBContext>();
            var mockSvc = new ArticleTypeSvc(mockContext.Object);
            var list = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="1",EditerName="123",CreateTime=DateTime.Now},
                new ArticleType(){ Id=1,Name="2",EditerName="223",CreateTime=DateTime.Now},
            };
            var dataSet = new Mock<DbSet<ArticleType>>().SetupList(list);
            mockContext.Setup(x => x.ArticleType).Returns(dataSet.Object);
            var data = mockSvc.GetPageData(new Service.QueryModel.ArticleTypeQueryModel());
            Assert.Equal(2, data.List.Count());
        }
    }
}
