using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Web.API
{
    [Produces("application/json")]
    [Route("API/Comment")]
    public class CommentController : Controller
    {
        private readonly ICommentSvc _commentSvc;
        public CommentController(ICommentSvc commentSvc)
        {
            _commentSvc = commentSvc;
        }

        [HttpPost("Add")]
        public JsonResult Add([FromBody]CommentDto dto)
        {
            var res = _commentSvc.AddSingle(dto);
            return Json(res);
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(CommentQueryModel qModel)
        {
            var res = await _commentSvc.GetPageDataAsync(qModel);
            return Json(res);
        }
    }
}