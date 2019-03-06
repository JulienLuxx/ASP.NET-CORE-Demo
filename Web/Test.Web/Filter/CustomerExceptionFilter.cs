using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Test.Web.Filter
{
    /// <summary>
    /// ExcetionFilterAttribute
    /// </summary>
    public class CustomerExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger _logger = null;
        private readonly IHostingEnvironment _environment = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        public CustomerExceptionFilter(ILogger<CustomerExceptionFilter> logger, IHostingEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var error = string.Empty;
            void ReadException(Exception ex)
            {
                error += string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
                if(null!=ex.InnerException)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(context.Exception);
            _logger.LogError(error);

            var result = new ContentResult
            {
                StatusCode = 500,
                ContentType = "text/json;charset=utf-8;"
            };

            if (_environment.IsDevelopment())
            {
                var json = new { message = exception.Message, detail = error };
                result.Content = JsonConvert.SerializeObject(json);
            }
            else
            {
                result.Content = "Sorry,Error!";
            }
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
