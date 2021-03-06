﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Core.Dto;
using Test.Core.Encrypt;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Enum;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Interface;

namespace Test.Service.Impl
{
    public class UserSvc : BaseSvc, IUserSvc
    {
        private IEncryptUtil _encryptUtil { get; set; }

        private IDbContextExtendSvc _dbContextExtendSvc { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="testDB"></param>
        /// <param name="encryptUtil"></param>
        /// <param name="dbContextExtendSvc"></param>
        public UserSvc(
            TestDBContext testDB, 
            IEncryptUtil encryptUtil,
            IDbContextExtendSvc dbContextExtendSvc
            ) : base(testDB)
        {
            _encryptUtil = encryptUtil;
            _dbContextExtendSvc = dbContextExtendSvc;
        }

        public ResultDto AddSingle(UserDto dto)
        {
            var result = new ResultDto();
            try
            {
                dto.SetDefaultValue();
                var randomStr = new Random().Next(100000).ToString();
                dto.Password = _encryptUtil.GetMd5By32(dto.Password + randomStr);
                var data = Mapper.Map<User>(dto);
                data.SaltValue = randomStr;
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0)
                {
                    result.ActionResult = true;
                    result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto> AddSingleAsync(UserDto dto)
        {
            var result = new ResultDto();
            try
            {
                dto.SetDefaultValue();
                var randomStr = new Random().Next(100000).ToString();
                dto.Password = _encryptUtil.GetMd5By32(dto.Password + randomStr);
                var data = Mapper.Map<User>(dto);
                data.SaltValue = randomStr;
                await _testDB.AddAsync(data);
                var flag = await _testDB.SaveChangesAsync();
                if (flag > 0)
                {
                    result.ActionResult = true;
                    result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var result = new ResultDto();
            try
            {
                if (!dto.NewPassword.Equals(dto.ConfirmPassword))
                {
                    result.Message = "UnConfirm";
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
                            result.Message = "OrigPassword error";
                            return result;
                        }
                        else
                        {
                            data.Password = _encryptUtil.GetMd5By32(dto.NewPassword + data.SaltValue);
                        }
                    }


                    var flag = await _dbContextExtendSvc.CommitTestAsync<TestDBContext, User>(_testDB, false);
                    if (flag > 0)
                    {
                        result.ActionResult = true;
                        result.Message = "Success";
                    }
                    else if (-1 == flag)
                    {
                        result.Message = "Data has been change , Please try again!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto<LoginUserDto>> LoginAsync(LoginDto dto)
        {
            var result = new ResultDto<LoginUserDto>();
            try
            {
                var data = await _testDB.User.AsNoTracking().Where(x => x.Name.Equals(dto.UserName) || x.Mobile.Equals(dto.UserName)).Select(s => new LoginUserDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Password = s.Password,
                    Status = s.Status,
                    SaltValue = s.SaltValue
                }).FirstOrDefaultAsync();

                if (null == data)
                {
                    result.Message = "User does not exist";
                    return result;
                }
                else if (!data.Password.Equals(_encryptUtil.GetMd5By32(dto.Password + data.SaltValue)))
                {
                    result.Message = "UserNameOrPassword error";
                    return result;
                }

                result.ActionResult = true;
                result.Message = "success";
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto> RegisterAsync(RegisterDto dto)
        {
            var result = new ResultDto();
            try
            {
                var mobileTask = _testDB.User.AsNoTracking().Where(x => x.Mobile.Equals(dto.Mobile)).AnyAsync();
                var mailBoxTask = _testDB.User.AsNoTracking().Where(x => x.MailBox.Equals(dto.MailBox)).AnyAsync();
                if (await mobileTask)
                {
                    result.Message = "MobileNumber already exist!";
                    return result;
                }
                if (await mailBoxTask)
                {
                    result.Message = "MailBox already exist!";
                    return result;
                }
                var userDto = Mapper.Map<UserDto>(dto);
                result = await AddSingleAsync(userDto);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
