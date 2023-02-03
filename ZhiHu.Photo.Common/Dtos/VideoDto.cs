using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Dtos.Bases;

namespace ZhiHu.Photo.Common.Dtos
{
    public class VideoDto: BaseDto
    {
        /// <summary>
        /// 回答id
        /// </summary>
        public int ImageId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string LUrl { get; set; }

        public string HUrl { get; set; }

        public string SUrl { get; set; }

    }
}
