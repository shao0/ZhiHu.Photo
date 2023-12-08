using System.Text.Json.Serialization;

namespace ZhiHu.Photo.Server.Models.ZhiHu
{
    public class UserInfo
    {

        /// <summary>
        /// 头像地址
        /// </summary>
        [JsonPropertyName("avatar_url")]
        public string? PortraitUrl { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonPropertyName("name")]
        public string? NikeName { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [JsonPropertyName("headline")]
        public string? Signature { get; set; }
    }
}
