using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Internal;
using System.Net;

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
                //var features = context.Features;
                await HandleLog(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private async Task HandleLog(HttpContext context)
        {
            var info = string.Empty;
            var jsonParam = string.Empty;
            var param = new Dictionary<string, string>();
            if (context.Request.ContentLength.HasValue)
            {
                var bytes = new byte[context.Request.Body.Length];
                context.Request.EnableRewind();
                context.Request.Body.Seek(0, 0);
                await context.Request.Body.ReadAsync(bytes, 0, bytes.Length);
                jsonParam= Encoding.UTF8.GetString(bytes);
                param = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonParam);
            }
            IDictionary<string, string> QueryDict = new Dictionary<string, string>();
            if (context.Request.QueryString.HasValue)
            {
                QueryDict = context.Request.Query.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault());
            }

            if (_environment.IsDevelopment())
            {
                info = JsonConvert.SerializeObject(new { ClientAddress = context.Connection.RemoteIpAddress.ToString()+":"+context.Connection.RemotePort.ToString(), RequestUrl = context.Request.Host + context.Request.Path, Query = QueryDict, Param = param });
                _logger.LogInformation(info);
            }

        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string error = string.Empty;

            void ReadException(Exception ex)
            {
                error += string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
                if (null != ex.InnerException) 
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(e);
            //_logger.LogError(error);

            if (_environment.IsDevelopment())
            {
                var json = new { message = e.Message, detail = error };
                error = JsonConvert.SerializeObject(json);
                _logger.LogError(error);
            }
            else
            {
                error = "抱歉，出错了";
            }

            await context.Response.WriteAsync(error);
        }
    }
}
