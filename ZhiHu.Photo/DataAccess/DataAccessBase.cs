using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Extensions;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.DataAccess.Contacts;
using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.DataAccess
{
    public class DataAccessBase<T> : IDataAccessBase<T>
    {
        private readonly HttpClient _client;
        public DataAccessBase(string url)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true  //忽略掉证书异常
            };
            _client = new HttpClient(httpClientHandler);
            _client.BaseAddress = new Uri(url);
        }
        public async Task<string> GetStringAsync(BaseRequest request)
        {
            var response = await Get(request);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<byte[]> GetBytesAsync(BaseRequest request)
        {
            var response = await Get(request);
            return await response.Content.ReadAsByteArrayAsync();
        }
        public async Task<Stream> GetStreamAsync(BaseRequest request)
        {
            var response = await Get(request);
            return await response.Content.ReadAsStreamAsync();
        }
        public async Task<string> PostStringAsync(BaseRequest request)
        {
            var result = await Post(request);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<byte[]> PostBytesAsync(BaseRequest request)
        {
            var result = await Post(request);
            return await result.Content.ReadAsByteArrayAsync();
        }
        public async Task<Stream> PostStreamAsync(BaseRequest request)
        {
            var content = new StringContent(request.ParameterJson, Encoding.UTF8, request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            return await result.Content.ReadAsStreamAsync();
        }

        public async Task<ApiResponse<T>> Added(BaseRequest request)
        {
            var result = await Post(request);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse<T>>();
        }

        public async Task<ApiResponse> Deleted(BaseRequest request)
        {
            var result = await Post(request);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse>();
        }

        public async Task<ApiResponse<T>> Updated(BaseRequest request)
        {
            var result = await Post(request);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse<T>>();
        }

        public async Task<ApiResponse<PagedList<T>>> GetList(BaseRequest request)
        {

            var result = await Get(request);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse<PagedList<T>>>();
        }

        private string MarkRequest => $"{GetType().Name}:Post请求****************************************************************************";
        private string MarkResponse => $"{GetType().Name}:Post响应****************************************************************************";
        private async Task<HttpResponseMessage> Get(BaseRequest request)
        {
            var url = $"{_client.BaseAddress.AbsoluteUri}{request.Route}";
            LogInfo(MarkRequest, url);
            var response = await _client.GetAsync(request.Route);
            LogInfo(MarkResponse, url, response.Content);
            return response;
        }

        private async Task<HttpResponseMessage> Post(BaseRequest request)
        {
            var url = $"{_client.BaseAddress.AbsoluteUri}{request.Route}";
            var content = new StringContent(request.ParameterJson, Encoding.UTF8, request.ContentType);
            LogInfo(MarkRequest, url, content);
            var response = await _client.PostAsync(request.Route, content);
            LogInfo(MarkResponse, url, response.Content);
            return response;
        }
        void LogInfo(string mark, string url, HttpContent? content = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(mark);
            sb.Append("地址URL:");
            sb.AppendLine(url);
            sb.Append("时间:");
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (content != null) sb.AppendLine(content.ReadAsStringAsync().GetAwaiter().GetResult());
            sb.AppendLine(mark);
            Debug.WriteLine(sb);
        }
    }
}
