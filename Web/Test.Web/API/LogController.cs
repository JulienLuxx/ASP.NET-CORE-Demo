using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Web.API
{
    [Produces("application/json", new string[] { "multipart/form-data", "application/x-www-form-urlencoded" })]
    [Route("API/Log")]
    public class LogController : Controller
    {
        private readonly ILogSvc _logSvc;
        public LogController(ILogSvc logSvc)
        {
            _logSvc = logSvc;
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(LogQueryModel qModel)
        {
            var resultTask = _logSvc.GetPageDataAsync(qModel);
            return Json(await resultTask);
        }
    }
}