using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Web.API
{
    [Authorize]
    [Produces("application/json")]
    [Route("API/Test")]
    public class TestController : Controller
    {
        private readonly ICommentSvc _commentSvc;
        public TestController(ICommentSvc commentSvc)
        {
            _commentSvc = commentSvc;
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(CommentQueryModel qModel)
        {
            var claims = User.Claims;
            var res = await _commentSvc.GetPageDataAsync(qModel);
            return Json(res);
        }
    }
}