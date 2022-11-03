using Newtonsoft.Json;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class UserInfo
    {

        /// <summary>
        /// 头像地址
        /// </summary>
        [JsonProperty("avatar_url")]
        public string? PortraitUrl { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string? NikeName { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [JsonProperty("headline")]
        public string? Signature { get; set; }
    }
}
