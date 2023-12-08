using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ZhiHuInfo
    {

        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public ZhiHuDataInfo[] Datas { get; set; }
        /// <summary>
        /// 下页信息
        /// </summary>
        [JsonPropertyName("paging")]
        public NextPageInfo NextPage { get; set; }
        public string Json { get; set; }
    }
}
