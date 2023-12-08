using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Attributes;
using ZhiHu.Photo.Helpers;

namespace ZhiHu.Photo.Extensions
{
    public static class AutoInjectionExtension
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dllNames">程序集名称</param>
        /// <param name="suffixNames">服务后缀</param>
        /// <returns></returns>
        public static IServiceCollection AutoInjection(this IServiceCollection services, string[]? dllNames = null, string[]? suffixNames = null)
        {
            if (suffixNames == null || suffixNames.Length == 0)
            {
                suffixNames = new[] { "Service" };
            }

            var types = AssemblyHelper
                .GetAssemblyTypes(dllNames)
                .Where(t =>
                    t is { IsClass: true, IsAbstract: false, IsInterface: false }
                    && suffixNames.Any(s => t.Name.EndsWith(s)));
            foreach (var t in types)
            {
                var _interface = t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}");
                if (_interface != null)
                {
                    RegistrationType(services, t, _interface);
                }
            }
            return services;
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="interfaceType"></param>
        private static void RegistrationType(IServiceCollection services, Type serviceType, Type interfaceType)
        {
            var attribute = serviceType.GetCustomAttribute<AutoInjectionAttribute>();
            var lifetime = ServiceLifetime.Transient;
            if (attribute != null)
            {
                if (attribute.Ignore) return;
                lifetime = attribute.Lifetime;
            }
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(interfaceType, serviceType);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(interfaceType, serviceType);
                    break;
                case ServiceLifetime.Transient:
                default:
                    services.AddTransient(interfaceType, serviceType);
                    break;
            }
        }
    }
}
