using Newtonsoft.Json;

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
        [JsonProperty("width")]
        public double Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; set; }
    }
}
