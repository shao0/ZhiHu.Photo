using System.ComponentModel.DataAnnotations.Schema;
using ZhiHu.Photo.Server.Entities.Bases;

namespace ZhiHu.Photo.Server.Entities
{
    [Table("Image")]
    public class ImageEntity : BaseEntity
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
        /// 关联回答
        /// </summary>
        [ForeignKey("AnswerId")]
        public AnswerEntity Answer { get; set; }
    }
}
