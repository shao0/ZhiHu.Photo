using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhiHu.Photo.Models.Bases
{
    public class BaseRequest
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public Method Method { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; } = "application/json";
        /// <summary>
        /// 参数
        /// </summary>
        public object Parameter { get; set; }
    }

    public enum Method
    {
        Get,
        Post,
    }
}
