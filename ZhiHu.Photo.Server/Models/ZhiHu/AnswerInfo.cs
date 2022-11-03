using Newtonsoft.Json;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class AnswerInfo
    {
        /// <summary>
        /// 摘录文本
        /// </summary>
        [JsonProperty("excerpt")]
        public string? Excerpt { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("content")]
        public string? Content { get; set; }
        /// <summary>
        /// 喜欢数
        /// </summary>
        [JsonProperty("thanks_count")]
        public int ThanksCount { get; set; }
        /// <summary>
        /// 赞同数
        /// </summary>
        [JsonProperty("voteup_count")]
        public int VoteUpCount { get; set; }
        /// <summary>
        /// 创建时间戳
        /// </summary>
        [JsonProperty("created_time")]
        public int CreatedTimeStamp { get; set; }
        /// <summary>
        /// 更新时间戳
        /// </summary>
        [JsonProperty("updated_time")]
        public int UpdatedTimeStamp { get; set; }
        /// <summary>
        /// 判断是否是视频
        /// </summary>
        [JsonProperty("attachment")]
        public object? Attachment { get; set; }
        /// <summary>
        /// 回答人
        /// </summary>
        [JsonProperty("author")]
        public UserInfo User { get; set; }
    }
}
