using Newtonsoft.Json;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ZhiHuInfo
    {

        /// <summary>
        /// 数据
        /// </summary>
        [JsonProperty("data")]
        public ZhiHuDataInfo[] Datas { get; set; }
        /// <summary>
        /// 下页信息
        /// </summary>
        [JsonProperty("paging")]
        public NextPageInfo NextPage { get; set; }
        public string Json { get; set; }
    }
}
