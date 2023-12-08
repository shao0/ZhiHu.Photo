using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.DataAccess.Contacts;
using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.DataAccess
{
    public class BaseDataAccess
    {
        private readonly HttpClient _client;
        public BaseDataAccess(string url)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true  //忽略掉证书异常
            };
            _client = new HttpClient(httpClientHandler);
            _client.BaseAddress = new Uri(url);

        }

        public async Task<string> GetString(BaseRequest request)
        {

            return await _client.GetStringAsync(request.Route);
        }

    }
}
