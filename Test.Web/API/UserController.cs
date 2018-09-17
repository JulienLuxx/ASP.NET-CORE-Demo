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

        [HttpPost("Register")]
        public async Task<JsonResult> Register(RegisterDto dto)
        {
            var resTask = _userSvc.Register(dto);
            return Json(await resTask);
        }
    }
}
