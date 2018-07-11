using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Web.Areas.PCAdmin.Controllers
{
    public class ManageController : Controller
    {
        private IArticleSvc _articleSvc;
        public ManageController(IArticleSvc articleSvc)
        {
            _articleSvc = articleSvc;
        }
        public IActionResult Index()
        {
            return View();
        }

        public Task<IActionResult> GetArticleListAsync()
        {
            return null;
        }

        public IActionResult GetArticleList(ArticleQueryModel qModel)
        {
            return null;
        }
    }
}