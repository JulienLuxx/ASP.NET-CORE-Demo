using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Web.API
{
    [Produces("application/json")]
    [Route("API/ArticleType")]
    public class ArticleTypeController : Controller
    {
        private readonly IArticleTypeSvc _articleTypeSvc;
        public ArticleTypeController(IArticleTypeSvc articleTypeSvc)
        {
            _articleTypeSvc = articleTypeSvc;
        }

        [HttpPost("Add")]
        public JsonResult Add(ArticleTypeDto dto)
        {
            var result = _articleTypeSvc.AddSingle(dto);
            return Json(result);
        }

        [HttpPost("Delete")]
        public JsonResult Delete(string idString)
        {
            var result = _articleTypeSvc.Delete(idString);
            return Json(result);
        }

        [HttpPost("Edit")]
        public JsonResult Edit([FromBody]ArticleTypeDto dto)
        {
            var result = _articleTypeSvc.Edit(dto);
            return Json(result);
        }

        [HttpPost("EditAsync")]
        public async Task<JsonResult> EditAsync(ArticleTypeDto dto)
        {
            var resultTask = _articleTypeSvc.EditAsync(dto);
            return Json(await resultTask);
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(ArticleTypeQueryModel qModel)
        {
            var result = await _articleTypeSvc.GetPageDataAsync(qModel);
            return Json(result);
        }

        [HttpGet("Detail")]
        public async Task<JsonResult> GetSingleDataAsync(int id)
        {
            var result = await _articleTypeSvc.GetSingleDataAsync(id);
            return Json(result);
        }
    }
}
