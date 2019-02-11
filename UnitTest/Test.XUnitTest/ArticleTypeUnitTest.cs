﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoqEFCoreExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Xunit;

namespace Test.XUnitTest
{
    public class ArticleTypeUnitTest
    {
        [Fact]
        public void AddSingle()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = new Mock<DbSet<ArticleType>>();
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);

            var svc = new ArticleTypeSvc(mockContext.Object);
            var data = new ArticleTypeDto() { Name = "233", EditerName = "test", CreateTime = DateTime.Now };
            svc.AddSingle(data);

            mockContext.Verify(x => x.Add(It.IsAny<ArticleType>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

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
