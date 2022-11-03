using Newtonsoft.Json;
using ZhiHu.Photo.Server.Models.ZhiHu.Bases;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ThumbnailDataInfo: SizeBase
    {

        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}
