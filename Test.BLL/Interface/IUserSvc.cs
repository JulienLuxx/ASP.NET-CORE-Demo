using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dto;
using Test.Service.Dto;

namespace Test.Service.Interface
{
    public interface IUserSvc
    {
        /// <summary>
        /// AddSingle
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultDto Add(UserDto dto);

        Task<ResultDto> ChangePasswordAsync(ChangePasswordDto dto);

        Task<ResultDto<LoginUserDto>> LoginAsync(LoginDto dto);

        Task<ResultDto> RegisterAsync(RegisterDto dto);
    }
}
