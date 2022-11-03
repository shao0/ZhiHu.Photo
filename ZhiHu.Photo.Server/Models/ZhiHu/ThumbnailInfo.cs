using Newtonsoft.Json;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ThumbnailInfo
    {
        /// <summary>
        /// 下页地址
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
        /// <summary>
        /// 缩略数据集
        /// </summary>
        [JsonProperty("thumbnails")]
        public ThumbnailDataInfo[]? Thumbnails { get; set; }
    }
}
