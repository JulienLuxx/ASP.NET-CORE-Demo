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
using Test.Core.Encrypt;

namespace Test.Service.Impl
{
    public class UserSvc : BaseSvc, IUserSvc
    {
        private IEncryptUtil _encryptUtil { get; set; }
        public UserSvc(IMapper mapper, TestDBContext testDB, IEncryptUtil encryptUtil) : base(mapper, testDB)
        {
            _encryptUtil = encryptUtil;
        }

        public ResultDto Add(UserDto dto)
        {
            var result = new ResultDto();
            try
            {
                dto.CreateTime = DateTime.Now;
                var randomStr = new Random().Next(100000).ToString();
                dto.Password = _encryptUtil.GetMd5By32(dto.Password + randomStr);
                var data = _mapper.Map<User>(dto);
                data.SaltValue = randomStr;
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0)
                {
                    result.ActionResult = true;
                    result.Msg = "Success";
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto> ChangePassword(ChangePasswordDto dto)
        {
            var result = new ResultDto();
            try
            {
                if (!dto.NewPassword.Equals(dto.ConfirmPassword))
                {
                    result.Msg = "UnConfirm";
                    return result;
                }
                var data = await _testDB.User.FindAsync(dto.Id);
                if (null != data) 
                {
                    dto.OrigPassword = _encryptUtil.GetMd5By32(dto.OrigPassword + data.SaltValue);
                    if (string.IsNullOrEmpty(data.Password))
                    {
                        data.Password = _encryptUtil.GetMd5By32(dto.NewPassword + data.SaltValue);
                    }
                    else
                    {
                        if (!dto.OrigPassword.Equals(data.Password))
                        {
                            result.Msg = "OrigPassword error";
                            return result;
                        }
                        else
                        {
                            data.Password = _encryptUtil.GetMd5By32(dto.NewPassword + data.SaltValue);
                        }
                    }


                    var flag = _testDB.SaveChanges();
                    if (flag > 0)
                    {
                        result.ActionResult = true;
                        result.Msg = "Success";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
            }
            return result;
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
