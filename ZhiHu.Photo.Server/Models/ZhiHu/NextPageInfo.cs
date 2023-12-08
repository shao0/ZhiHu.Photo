using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    /// <summary>
    /// 下一页信息
    /// </summary>
    public class NextPageInfo
    {
        /// <summary>
        /// 第几页
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }
        /// <summary>
        /// 是否最后一页
        /// </summary>
        [JsonPropertyName("is_end")]
        public bool IsEnd { get; set; }
        /// <summary>
        /// 下页地址
        /// </summary>
        [JsonPropertyName("next")]
        public string? NextUrl { get; set; }
        
    }
}
