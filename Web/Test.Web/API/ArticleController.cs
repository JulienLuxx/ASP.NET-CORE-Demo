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

        [HttpPost("Adds")]
        public JsonResult Add(string dataJson)
        {
            var result = _articleSvc.AddSingle(dataJson);
            return Json(result);
        }

        [HttpPost("Add")]
        public JsonResult Add([FromBody]ArticleDto dto)
        {
            var result = _articleSvc.AddSingle(dto);
            return Json(result);
        }

        [HttpPost("Delete")]
        public JsonResult Delete(string idString)
        {
            var result = _articleSvc.Delete(idString);
            return Json(result);
        }

        [HttpPost("Edits")]
        public JsonResult Edit(string dataJson)
        {
            var result = _articleSvc.Edit(dataJson);
            return Json(result);
        }

        [HttpPost("Edit")]
        public JsonResult Edit([FromBody]ArticleDto dto)
        {
            var result = _articleSvc.Edit(dto);
            return Json(result);
        }

        [HttpGet("Detail")]
        public async Task<JsonResult> GetDetail(int id)
        {
            var res = await _articleSvc.GetSingleDataAsync(id);
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