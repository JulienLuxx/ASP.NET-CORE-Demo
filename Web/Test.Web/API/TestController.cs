using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Dto;
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
        private readonly IArticleTypeSvc _articleTypeSvc;
        public TestController(ICommentSvc commentSvc,IArticleTypeSvc articleTypeSvc)
        {
            _commentSvc = commentSvc;
            _articleTypeSvc = articleTypeSvc;
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(CommentQueryModel qModel)
        {
            var id = UserId;
            var name = UserName;

            var res = await _commentSvc.GetPageDataAsync(qModel);
            return Json(res);
        }

        [AllowAnonymous]
        [HttpPost("Edit")]
        public async Task<JsonResult> EditAsync(ArticleTypeDto dto)
        {
            var resultTask = _articleTypeSvc.EditAsync(dto);
            return Json(await resultTask);
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