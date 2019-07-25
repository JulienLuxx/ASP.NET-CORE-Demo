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
            var guid = Guid.NewGuid();
            try
            {
                await _next.Invoke(context);
                //var features = context.Features;

                await HandleLog(guid, context);
                
            }
            catch (Exception e)
            {
                await HandleException(guid, context, e);
            }
        }

        private async Task HandleLog(Guid guid, HttpContext context) 
        {
            var info = string.Empty;
            //var jsonParam = string.Empty;
            var param = new Dictionary<string, string>();
            if (context.Request.ContentLength.HasValue)
            {
                if (context.Request.HasFormContentType)
                {
                    var form = context.Request.Form;
                    param = form.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault());
                }
                else if (context.Request.ContentLength > 0) 
                {
                    var bytes = new byte[context.Request.Body.Length];
                    context.Request.EnableRewind();
                    context.Request.Body.Seek(0, 0);
                    await context.Request.Body.ReadAsync(bytes, 0, bytes.Length);
                    var jsonParam = Encoding.UTF8.GetString(bytes);
                    param = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonParam);
                }
            }
            if (context.Request.QueryString.HasValue)
            {
                param = context.Request.Query.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault());
            }
            info = JsonConvert.SerializeObject(new { Id = guid, ClientAddress = context.Connection.RemoteIpAddress.ToString() + ":" + context.Connection.RemotePort.ToString(), RequestUrl = context.Request.Host + context.Request.Path, Param = param });
            _logger.LogInformation(info);
        }

        private async Task HandleException(Guid guid, HttpContext context, Exception exception) 
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

            ReadException(exception);

            if (_environment.IsDevelopment())
            {
                error = JsonConvert.SerializeObject(new
                {
                    Id = guid,
                    message = exception.Message,
                    detail = error
                });
                _logger.LogError(error);
            }
            else
            {
                error = JsonConvert.SerializeObject(new
                {
                    Id = guid,
                    message = exception.Message,
                    detail = error
                });
                _logger.LogError(error);
                error = "抱歉，出错了";
            }

            await context.Response.WriteAsync(error);
        }
    }
}
