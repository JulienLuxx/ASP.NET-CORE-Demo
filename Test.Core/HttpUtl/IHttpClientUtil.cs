using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.HttpUtl
{
    public interface IHttpClientUtil
    {
        Task<dynamic> SendAsync(dynamic param, string url, HttpMethod httpMethod, MediaTypeEnum mediaType);
    }
}
