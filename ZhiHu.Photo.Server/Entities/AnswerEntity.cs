using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ZhiHu.Photo.Server.Entities.Bases;
using ZhiHu.Photo.Server.Models.Attributes;

namespace ZhiHu.Photo.Server.Entities
{
    [Table("Answer")]
    public class AnswerEntity:BaseEntity
    {
        /// <summary>
        /// 头像地址
        /// </summary>
        public string? PortraitUrl { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [QueryString]
        public string? NikeName { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [QueryString]
        public string? Signature { get; set; }

        /// <summary>
        /// 摘录文本
        /// </summary>
        [QueryString]
        public string? Excerpt { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 媒体数量
        /// </summary>
        public int MediaCount { get; set; }

        /// <summary>
        /// 喜欢数
        /// </summary>
        public int ThanksCount { get; set; }

        /// <summary>
        /// 赞同数
        /// </summary>
        public int VoteUpCount { get; set; }

        /// <summary>
        /// 回答创建时间戳
        /// </summary>
        public int AnswerCreatedTimeStamp { get; set; }

        /// <summary>
        /// 回答更新时间戳
        /// </summary>
        public int AnswerUpdatedTimeStamp { get; set; }

        /// <summary>
        /// 判断是否是视频
        /// </summary>
        public bool Attachment { get; set; }

        /// <summary>
        /// 下一页地址
        /// </summary>
        public string? NextPageUrl { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 图片集合
        /// </summary>
        public IEnumerable<ImageEntity>? Images { get; set; } 

    }
}
