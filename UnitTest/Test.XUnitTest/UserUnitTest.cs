using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoqEFCoreExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Encrypt;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Enum;
using Test.Service.Dto;
using Test.Service.Impl;
using Test.Service.Infrastructure;
using Xunit;

namespace Test.XUnitTest
{
    public class UserUnitTest
    {
        private List<User> _sampleList;
        private EncryptUtil _encryptUtil;
        public UserUnitTest()
        {
            _encryptUtil = new EncryptUtil();
            _sampleList = new List<User>()
            {
                new User(){ Id=1,CreateTime=DateTime.Now,Name="Test01", SaltValue="1", Password=_encryptUtil.GetMd5By32("test01"+"1"),Status=UserStatusEnum.Activate.GetHashCode(),MailBox="Test01@qq.com",Mobile="123123"}
            };
        }

        [Fact]
        public async Task RegisterAsyncTest()
        {
            Mapper.Initialize(x => x.AddProfile<CustomizeProfile>());
            var mockSet = new Mock<DbSet<User>>().SetupList(_sampleList);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.User).Returns(mockSet.Object);
            var mockSvc = new UserSvc(mockContext.Object,_encryptUtil);

            var dto = new RegisterDto() { Name = "RegisterTest", Password = "registertest", Mobile = "123456", MailBox = "RegisterTest@qq.com" };
            var result = await mockSvc.RegisterAsync(dto);

            mockContext.Verify(x => x.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Fact]
        public async Task ChangePasswordAsyncTest()
        {
            var mockSet = new Mock<DbSet<User>>().SetupList(_sampleList);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.User).Returns(mockSet.Object);
            var mockSvc = new UserSvc(mockContext.Object, _encryptUtil);

            var dto = new ChangePasswordDto() { Id = 1, OrigPassword = "test01", NewPassword = "changepw", ConfirmPassword = "changepw" };
            await mockSvc.ChangePasswordAsync(dto);

            mockContext.Verify(x => x.SaveChanges(), Times.Once());
            var result = mockSet.Object.Where(x => x.Id == 1).Select(s => s.Password).FirstOrDefault();
            var pw = _encryptUtil.GetMd5By32("changepw" + "1");
            Assert.Equal(result, pw);
        }

        [Fact]
        public async Task LoginAsyncTest()
        {
            var mockSet = new Mock<DbSet<User>>().SetupList(_sampleList);
            var mockContext = new Mock<TestDBContext>();
            mockContext.Setup(x => x.User).Returns(mockSet.Object);
            var mockSvc = new UserSvc(mockContext.Object, _encryptUtil);

            var dto = new LoginDto() { UserName = "Test01", Password = "test01",  };
            var result= await mockSvc.LoginAsync(dto);

            Assert.False(!result.ActionResult);
        }
    }
}
