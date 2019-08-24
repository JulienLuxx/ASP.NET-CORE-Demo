using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Map;

namespace Test.Core.HttpUtl
{
    public class HttpClientUtil : IHttpClientUtil 
    {
        private IHttpClientFactory _clientFactory { get; set; }
        private IMapUtil _mapUtil { get; set; }
        public HttpClientUtil(IHttpClientFactory clientFactory,IMapUtil mapUtil)
        {
            _clientFactory = clientFactory;
            _mapUtil = mapUtil;
        }

        public async Task<dynamic> SendAsync(dynamic param, string url, HttpMethod httpMethod, MediaTypeEnum mediaType) 
        {
            var request = new HttpRequestMessage(httpMethod, @url);
            if ((HttpMethod.Get.Equals(httpMethod)))
            {
                var dict = _mapUtil.DynamicToDictionary(param);
                switch (mediaType)
                {
                    case MediaTypeEnum.UrlQuery:
                        var paramUrl = QueryHelpers.AddQueryString(@url, dict);
                        request.RequestUri = new Uri(paramUrl);
                        break;
                    case MediaTypeEnum.ApplicationFormUrlencoded:
                        request.Content = new FormUrlEncodedContent(dict);
                        break;
                }

            }
            else if (HttpMethod.Post.Equals(httpMethod))
            {
                var dict = _mapUtil.DynamicToDictionary(param);
                switch (mediaType)
                {
                    case MediaTypeEnum.ApplicationFormUrlencoded:                        
                        request.Content = new FormUrlEncodedContent(dict);
                        break;
                    case MediaTypeEnum.ApplicationJson:
                        var jsonParam = JsonConvert.SerializeObject(param);
                        request.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                        break;
                    case MediaTypeEnum.MultipartFormData:
                        var content = new MultipartFormDataContent();                        
                        foreach (var item in dict)
                        {
                            content.Add(new StringContent(item.Value), item.Key);
                        }
                        request.Content = content;
                        break;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
