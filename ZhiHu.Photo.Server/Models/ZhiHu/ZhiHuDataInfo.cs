using Newtonsoft.Json;
using ZhiHu.Photo.Server.Entities;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class ZhiHuDataInfo
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        [JsonProperty("target_type")]
        public string TargetType { get; set; }
        /// <summary>
        /// 回答信息
        /// </summary>
        [JsonProperty("target")]
        public AnswerInfo Answer { get; set; }
        /// <summary>
        /// 回答信息
        /// </summary>
        [JsonProperty("thumbnail_info")]
        public ThumbnailInfo[] ThumbnailInfos { get; set; }
        /// <summary>
        /// 回答信息
        /// </summary>
        [JsonProperty("thanks_count")]
        public int MediaCount { get; set; }


        public static implicit operator AnswerEntity(ZhiHuDataInfo dataInfo)
        {
            var entity = new AnswerEntity();
            entity.PortraitUrl = dataInfo.Answer.User.PortraitUrl;
            entity.NikeName = dataInfo.Answer.User.NikeName;
            entity.Signature = dataInfo.Answer.User.Signature;
            entity.Excerpt = dataInfo.Answer.Excerpt;
            entity.Content = dataInfo.Answer.Content;
            entity.MediaCount = dataInfo.MediaCount;
            entity.ThanksCount = dataInfo.Answer.ThanksCount;
            entity.VoteUpCount = dataInfo.Answer.VoteUpCount;
            entity.AnswerCreatedTimeStamp = dataInfo.Answer.CreatedTimeStamp;
            entity.AnswerUpdatedTimeStamp = dataInfo.Answer.UpdatedTimeStamp;
            entity.Attachment = dataInfo.Answer.Attachment != null;

            return entity;
        }
    }
}
