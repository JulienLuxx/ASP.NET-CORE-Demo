using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Service.Dto
{
    public class UserDto : BaseDto
    {
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
}
