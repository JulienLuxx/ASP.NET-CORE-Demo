using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Core.Map;
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

        private IHttpClientFactory _clientFactory { get; set; }

        private IMapUtil _mapUtil { get; set; }

        public TestController(ICommentSvc commentSvc, IHttpClientFactory clientFactory, IMapUtil mapUtil)  
        {
            _commentSvc = commentSvc;
            _clientFactory = clientFactory;
            _mapUtil = mapUtil;
        }

        [HttpGet("Page")]
        public async Task<JsonResult> GetPageAsync(CommentQueryModel qModel)
        {
            var id=UserId;
            var name = UserName;

            var res = await _commentSvc.GetPageDataAsync(qModel);
            return Json(res);
        }

        [AllowAnonymous]
        [HttpGet("LogTest")]
        public async Task<JsonResult> LogTest()
        {
            throw new Exception("Error!");
            return Json(new { value1 = "", value2 = "" });
        }

        [AllowAnonymous]
        [HttpGet("HttpClientGetTest")]
        public async Task<dynamic> HttpClientGetTestAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, @"http://localhost:54238/API/ArticleType/Page");
            var param = new ArticleTypeQueryModel() { PageSize = 1 };
            
            var jsonParam = JsonConvert.SerializeObject(param);
            //request.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
            var dict = _mapUtil.DynamicToDictionary(param);
            request.Content = new FormUrlEncodedContent(dict);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return result;
            }
            else
            {
                return "error";
            }
        }

        [AllowAnonymous]
        [HttpGet("HttpClientPostTest")]
        public async Task<dynamic> HttpClientPostTestAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, @"http://localhost:54238/API/ArticleType/AddAsync");
            var param = new ArticleTypeDto()
            {
                Name = "HttpClientTest",
                EditerName = "HttpClient",
                Status = 1,
                CreateTime = DateTime.Now
            };
            var jsonParam = JsonConvert.SerializeObject(param);
            request.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return result;
            }
            else
            {
                return "error";
            }
        }
    }
}