﻿using System.Collections.Generic;
using System.Windows.Documents;
using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.Models
{
    public class AnswerInfo : BaseInfo
    {
        /// <summary>
        /// 头像地址
        /// </summary>
        public string? PortraitUrl { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? NikeName { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string? Signature { get; set; }

        /// <summary>
        /// 摘录文本
        /// </summary>
        public string? Excerpt { get; set; }

        /// <summary>
        /// 媒体数量
        /// </summary>
        public string? MediaCount { get; set; }

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


        public ImageInfo[]? Images { get; set; }

        /// <summary>
        /// 详细
        /// </summary>
        public List<Information> InfoList { get; set; } = new();
    }
}
