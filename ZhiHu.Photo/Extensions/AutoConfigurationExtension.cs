using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Common.Extensions;
using ZhiHu.Photo.Helpers;

namespace ZhiHu.Photo.Extensions
{
    public static class AutoConfigurationExtension
    {
        private static string Path = $"{AppDomain.CurrentDomain.BaseDirectory}Configurations";
        public static IServiceCollection AddConfiguration<T>(this IServiceCollection service)
        {
            var type = typeof(T);
            var path = $"{Path}/{type.Name}.json";
            if (!File.Exists(path)) return service;
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return service;
            var o = json.ToObject<T>();
            service.AddSingleton(type, o);
            return service;
        }
        public static IServiceCollection AddConfiguration(this IServiceCollection service, Type type)
        {
            var path = $"{Path}/{type.Name}.json";
            if (!File.Exists(path)) return service;
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return service;
            var o = json.ToObject(type);
            service.AddSingleton(type, o);
            return service;
        }

        public static IServiceCollection AutoConfiguration(this IServiceCollection service, string[] namespaceNames, string[]? dllNames = null)
        {
            var types = AssemblyHelper
                .GetAssemblyTypes(dllNames)
                .Where(t => namespaceNames.Any(n => t.Namespace != null && t.Namespace.StartsWith(n)));
            foreach (var t in types)
            {
                service.AddConfiguration(t);
            }
            return service;
        }
    }
}
