using ZhiHu.Photo.Models.Bases;

namespace ZhiHu.Photo.Models
{
    public class VideoInfo: BaseInfo
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
