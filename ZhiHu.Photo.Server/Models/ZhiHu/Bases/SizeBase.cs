
using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu.Bases
{
    /// <summary>
    /// 大小
    /// </summary>
    public class SizeBase
    {

        /// <summary>
        /// 宽
        /// </summary>
        [JsonPropertyName("width")]
        public double Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [JsonPropertyName("height")]
        public double Height { get; set; }
    }
}
