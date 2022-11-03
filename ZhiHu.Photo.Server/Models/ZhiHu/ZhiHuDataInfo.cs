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


        public static implicit operator AnswerEntity(ZhiHuDataInfo DataInfo)
        {
            var entity = new AnswerEntity();
            entity.PortraitUrl = DataInfo.Answer.User.PortraitUrl;
            entity.NikeName = DataInfo.Answer.User.NikeName;
            entity.Signature = DataInfo.Answer.User.Signature;
            entity.Excerpt = DataInfo.Answer.Excerpt;
            entity.Content = DataInfo.Answer.Content;
            entity.MediaCount = DataInfo.MediaCount;
            entity.ThanksCount = DataInfo.Answer.ThanksCount;
            entity.VoteUpCount = DataInfo.Answer.VoteUpCount;
            entity.AnswerCreatedTimeStamp = DataInfo.Answer.CreatedTimeStamp;
            entity.AnswerUpdatedTimeStamp = DataInfo.Answer.UpdatedTimeStamp;
            entity.Attachment = DataInfo.Answer.Attachment != null;

            return entity;
        }
    }
}
