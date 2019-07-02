using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Test.Web
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostingEnvironment environment)
        {
            this._next = next;
            this._logger = logger;
            this._environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                var features = context.Features;
                await HandleLog(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleLog(HttpContext context)
        {

        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string error = "";

            void ReadException(Exception ex)
            {
                error += string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
                if (ex.InnerException != null)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(e);
            _logger.LogError(error);

            if (_environment.IsDevelopment())
            {
                var json = new { message = e.Message, detail = error };
                error = JsonConvert.SerializeObject(json);
            }
            else
            {
                error = "抱歉，出错了";
            }

            await context.Response.WriteAsync(error);
        }
    }
}
