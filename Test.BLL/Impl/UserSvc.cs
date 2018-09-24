using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Enum;
using Test.Service.Dto;
using Test.Service.Interface;

namespace Test.Service.Impl
{
    public class UserSvc : BaseSvc, IUserSvc
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="testDB"></param>
        public UserSvc(IMapper mapper, TestDBContext testDB) : base(mapper, testDB)
        {
        }

        public ResultDto Add(UserDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _mapper.Map<User>(dto);
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0) 
                {
                    res.ActionResult = true;
                    res.Msg = "Success";
                }
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        public async Task<ResultDto> Register(RegisterDto dto)
        {
            var res = new ResultDto();
            try
            {
                if (await _testDB.User.Where(x => x.Mobile.Equals(dto.Mobile)).AnyAsync())
                {
                    return res;
                }
                var data = _mapper.Map<UserDto>(dto);
                data.Status = UserStatusEnum.Activate.GetHashCode();
                var flag = Add(data);
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }
    }
}
