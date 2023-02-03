using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.Models
{
    public class ImageInfo:BaseInfo
    {
        /// <summary>
        /// 回答id
        /// </summary>
        public int AnswerId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public VideoInfo? Video { get; set; }


    }
}
