using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Interface;
using Test.Service.QueryModel;
using Test.Web.Base;
using Test.Web.Filter;

namespace Test.Web.API
{
    [Authorize]
    [Produces("application/json")]
    [Route("API/Test")]
    [ServiceFilter(typeof(CustomerExceptionFilter))]
    public class TestController : BaseController
    {
        private readonly ICommentSvc _commentSvc;
        public TestController(ICommentSvc commentSvc)
        {
            _commentSvc = commentSvc;
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(CommentQueryModel qModel)
        {
            var id=UserId;
            var name = UserName;

            var res = await _commentSvc.GetPageDataAsync(qModel);
            return Json(res);
        }

        [AllowAnonymous]
        [HttpPost("PostTest")]
        public async Task<JsonResult> PostTest(string Msg)
        {
            return Json(Msg);
        }

        [AllowAnonymous]
        [HttpGet("LogTest")]
        public async Task<JsonResult> LogTest()
        {
            throw new Exception("Error!");
            return Json(new { value1 = "", value2 = "" });
        }
    }
}