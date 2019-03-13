using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dto;
using Test.Core.IOC;
using Test.Service.Dto;

namespace Test.Service.Interface
{
    public interface IUserSvc : IDependency 
    {
        ResultDto AddSingle(UserDto dto);

        Task<ResultDto> AddSingleAsync(UserDto dto);

        Task<ResultDto> ChangePasswordAsync(ChangePasswordDto dto);

        Task<ResultDto<LoginUserDto>> LoginAsync(LoginDto dto);

        Task<ResultDto> RegisterAsync(RegisterDto dto);
    }
}
