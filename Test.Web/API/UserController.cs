using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Service.Dto;
using Test.Service.Interface;

namespace Test.Web.API
{
    [Produces("application/json")]
    [Route("API/User")]
    public class UserController : Controller
    {
        private readonly IUserSvc _userSvc;
        public UserController(IUserSvc userSvc)
        {
            _userSvc = userSvc;
        }

        [HttpPost("ChangePassword")]
        public async Task<JsonResult> ChangePassword(ChangePasswordDto dto)
        {
            var resultTask = _userSvc.ChangePasswordAsync(dto);
            return Json(await resultTask);
        }

        [HttpPost("Register")]
        public async Task<JsonResult> Register(RegisterDto dto)
        {
            var resultTask = _userSvc.RegisterAsync(dto);
            return Json(await resultTask);
        }

        [HttpPost("Login")]
        public async Task<JsonResult> Login(LoginDto dto)
        {
            var resultTask = _userSvc.LoginAsync(dto);
            return Json(await resultTask);
        }
    }
}
