using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhiHu.Photo.Common.Extensions;

namespace ZhiHu.Photo.Models.Bases
{
    public class BaseRequest
    {
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

        public string ParameterJson => Parameter.ToJson();
    }

}
