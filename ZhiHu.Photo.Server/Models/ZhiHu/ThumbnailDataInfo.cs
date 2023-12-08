using System.Text.Json.Serialization;
using ZhiHu.Photo.Server.Models.ZhiHu.Bases;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ThumbnailDataInfo: SizeBase
    {

        /// <summary>
        /// 地址
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
