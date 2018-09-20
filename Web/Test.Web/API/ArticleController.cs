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
    [Route("API/Article")]
    public class ArticleController : Controller
    {
        private readonly IArticleSvc _articleSvc;
        public ArticleController(IArticleSvc articleSvc)
        {
            _articleSvc = articleSvc;
        }

        [HttpPost("Add")]
        public JsonResult Add(ArticleDto dto)
        {
            var res = _articleSvc.AddSingle(dto);
            return Json(res);
        }

        [HttpGet("Detail")]
        public async Task<JsonResult> GetDetail(int id)
        {
            var res = await _articleSvc.GetSingleDataAsync(id);
            return Json(res);
        }

        [HttpPost("Edit")]
        public JsonResult Edit(ArticleDto dto)
        {
            var res = _articleSvc.Edit(dto);
            return Json(res);
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(ArticleQueryModel qModel)
        {
            var res = await _articleSvc.GetPageDataAsync(qModel);
            return Json(res);
        }

    }
}