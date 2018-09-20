using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Dto;
using Test.Service.Interface;

namespace Test.Web.Areas.PCAdmin.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleSvc _articleSvc;
        public ArticleController(IArticleSvc articleSvc)
        {
            _articleSvc = articleSvc;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add(ArticleDto dto)
        {
            var data = _articleSvc.AddSingle(dto);
            return Json(data);
        }

        public async Task<IActionResult> AddAsync(ArticleDto dto)
        {
            var data = await _articleSvc.AddSingleAsync(dto);
            return Json(data);
        }
    }
}