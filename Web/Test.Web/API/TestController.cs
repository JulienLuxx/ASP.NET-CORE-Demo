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

namespace Test.Web.API
{
    //[Authorize]
    [Produces("application/json")]
    [Route("API/Test")]
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

        [HttpPost("Edit")]
        public async Task<JsonResult> EditAsync(ArticleTypeDto dto)
        {
            var resultTask = _articleTypeSvc.EditAsync(dto);
            return Json(await resultTask);
        }
    }
}