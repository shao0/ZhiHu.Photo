using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class AnswerInfo
    {
        /// <summary>
        /// 摘录文本
        /// </summary>
        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        /// <summary>
        /// 喜欢数
        /// </summary>
        [JsonPropertyName("thanks_count")]
        public int ThanksCount { get; set; }
        /// <summary>
        /// 赞同数
        /// </summary>
        [JsonPropertyName("voteup_count")]
        public int VoteUpCount { get; set; }
        /// <summary>
        /// 创建时间戳
        /// </summary>
        [JsonPropertyName("created_time")]
        public int CreatedTimeStamp { get; set; }
        /// <summary>
        /// 更新时间戳
        /// </summary>
        [JsonPropertyName("updated_time")]
        public int UpdatedTimeStamp { get; set; }
        /// <summary>
        /// 判断是否是视频
        /// </summary>
        [JsonPropertyName("attachment")]
        public object? Attachment { get; set; }
        /// <summary>
        /// 回答人
        /// </summary>
        [JsonPropertyName("author")]
        public UserInfo User { get; set; }
    }
}
