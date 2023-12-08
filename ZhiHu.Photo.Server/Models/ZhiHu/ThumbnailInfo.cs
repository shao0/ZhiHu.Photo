using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ThumbnailInfo
    {
        /// <summary>
        /// 下页地址
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
        /// <summary>
        /// 缩略数据集
        /// </summary>
        [JsonPropertyName("thumbnails")]
        public ThumbnailDataInfo[]? Thumbnails { get; set; }
    }
}
