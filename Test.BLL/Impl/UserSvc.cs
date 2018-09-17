using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Test.Service.Impl
{
    public class UserSvc : BaseSvc, IUserSvc
    {
        public UserSvc(IMapper mapper, TestDBContext testDB) : base(mapper, testDB)
        {
        }

        public ResultDto Add(UserDto dto)
        {
            var res = new ResultDto();
            try
            {
                dto.CreateTime = DateTime.Now;
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
            var result = new ResultDto();
            try
            {
                var mobileTask = _testDB.User.AsNoTracking().Where(x => x.Mobile.Equals(dto.Mobile)).AnyAsync();
                var mailBoxTask = _testDB.User.AsNoTracking().Where(x => x.MailBox.Equals(dto.MailBox)).AnyAsync();
                if (await mobileTask)
                {
                    return result;
                }
                if (await mailBoxTask)
                {
                    return result;
                }
                var userDto = _mapper.Map<UserDto>(dto);
                result = Add(userDto);
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
            }
            return result;
        }
    }
}
