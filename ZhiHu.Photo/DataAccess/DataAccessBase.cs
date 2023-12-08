using System;
using System.Collections.Generic;
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
            return await _client.GetStringAsync(request.Route);
        }
        public async Task<byte[]> GetBytesAsync(BaseRequest request)
        {
            return await _client.GetByteArrayAsync(request.Route);
        }
        public async Task<Stream> StreamAsync(BaseRequest request)
        {
            return await _client.GetStreamAsync(request.Route);
        }
        public async Task<string> PostStringAsync(BaseRequest request)
        {
            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8, request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<byte[]> PostBytesAsync(BaseRequest request)
        {

            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8,  request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            return await result.Content.ReadAsByteArrayAsync();
        }
        public async Task<Stream> PostStreamAsync(BaseRequest request)
        {
            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8,  request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            return await result.Content.ReadAsStreamAsync();
        }

        public async Task<ApiResponse<T>> Added(BaseRequest request)
        {
            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8,  request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse<T>>();
        }

        public async Task<ApiResponse> Deleted(BaseRequest request)
        {
            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8,  request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse>();
        }

        public async Task<ApiResponse<T>> Updated(BaseRequest request)
        {
            var content = new StringContent(request.Parameter.ToJson(), Encoding.UTF8,  request.ContentType);
            var result = await _client.PostAsync(request.Route, content);
            var json = await result.Content.ReadAsStringAsync();
            return json.ToObject<ApiResponse<T>>();
        }

        public async Task<ApiResponse<PagedList<T>>> GetList(BaseRequest request)
        {
            var json = await _client.GetStringAsync(request.Route);
            return json.ToObject<ApiResponse<PagedList<T>>>();
        }
    }
}
