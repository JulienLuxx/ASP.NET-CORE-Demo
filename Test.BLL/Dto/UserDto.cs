using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Service.Dto
{
    public class UserDto : BaseDto
    {

        public UserDto()
        {
            CreateTime = DateTime.Now;
            Status = -1;
        }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }

        public string Mobile { get; set; }

        public string MailBox { get; set; }
    }

    public class RegisterDto
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Mobile { get; set; }

        public string MailBox { get; set; }
    }

    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string JumpUrl { get; set; }

        public string ClientId { get; set; }
    }

    public class LoginUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }

        public string SaltValue { get; set; }
    }

    public class ChangePasswordDto
    {
        public int Id { get; set; }

        public string OrigPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
