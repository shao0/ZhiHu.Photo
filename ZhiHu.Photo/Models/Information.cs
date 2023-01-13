using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhiHu.Photo.Models
{
    public class Information
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoType InfoType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum InfoType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
    }
}
