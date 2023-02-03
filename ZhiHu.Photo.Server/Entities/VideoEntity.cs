using System.ComponentModel.DataAnnotations.Schema;
using ZhiHu.Photo.Server.Entities.Bases;

namespace ZhiHu.Photo.Server.Entities
{
    [Table("Video")]
    public class VideoEntity : BaseEntity
    {
        /// <summary>
        /// 回答id
        /// </summary>
        public int ImageId { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string? LUrl { get; set; }

        public string? HUrl { get; set; }

        public string? SUrl { get; set; }

    }
}
