using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoqEFCoreExtension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Xunit;

namespace TestProject.XUnitTest
{
    public class ArticleTypeUnitTest
    {
        private List<ArticleType> _sampleList;

        public ArticleTypeUnitTest()
        {
            var date = DateTime.Now;
            _sampleList = new List<ArticleType>()
            {
                new ArticleType(){ Id=1,Name="Test01",EditerName="TestUser",IsDeleted=false,CreateTime=date,Timestamp=BitConverter.GetBytes(date.Ticks) },
            };
        }

        //Failed
        [Fact]
        public async Task EditAsyncTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = new Mock<DbSet<ArticleType>>().SetupList(_sampleList);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.ArticleType).Returns(mockSet.Object);
            var mockSvc = new ArticleTypeSvc(mockContext.Object,new DbContextExtendSvc());

            var dto1 = new ArticleTypeDto() { Id = 1, Name = "TestA", EditerName = "TestUserA", CreateTime = DateTime.Now, IsDeleted = false };
            var dto2 = new ArticleTypeDto() { Id = 2, Name = "TestB", EditerName = "TestUserB", CreateTime = DateTime.Now, IsDeleted = false };

            var task1 = await mockSvc.EditAsync(dto1);
            var task2 = await mockSvc.EditAsync(dto2);
            //var result1 = await task1;
            //var result2 = await task2;
        }
    }
}
