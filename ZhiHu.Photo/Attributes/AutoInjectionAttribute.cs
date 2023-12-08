using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ZhiHu.Photo.Attributes
{
    public class AutoInjectionAttribute:Attribute
    {
        /// <summary>
        /// 忽略服务
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime Lifetime { get; set; }

        public AutoInjectionAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
