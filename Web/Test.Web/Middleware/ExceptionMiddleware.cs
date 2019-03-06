using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Test.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="requestDelegate"></param>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, IHostingEnvironment environment)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate.Invoke(context);
                var features = context.Features;
            }
            catch (Exception e)
            {
                await HandleExcetion(context, e);
            }
        }

        private async Task HandleExcetion(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8";
            var error = string.Empty;

            void ReadException(Exception ex)
            {
                error += string.Format("{0}  |  {1}  |  {2}", ex.Message, ex.StackTrace, ex.InnerException);
                if (null != ex.InnerException)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(exception);
            if (_environment.IsDevelopment())
            {
                var json = new { message = exception.Message, detail = error };
                error = JsonConvert.SerializeObject(json);
            }
            else
            {
                error = "Sorry,Error!";
            }
            await context.Response.WriteAsync(error);
        }
    }
}
