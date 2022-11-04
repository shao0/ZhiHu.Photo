using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Dtos.Bases;

namespace ZhiHu.Photo.Common.Dtos
{
    public class ImageDto : BaseDto
    {
        /// <summary>
        /// 回答id
        /// </summary>
        public int AnswerId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }
    }
}
