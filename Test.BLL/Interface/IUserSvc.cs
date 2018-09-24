﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Service.Dto;

namespace Test.Service.Interface
{
    public interface IUserSvc
    {
        ResultDto Add(UserDto dto);

        Task<ResultDto> Register(RegisterDto dto);
    }
}
