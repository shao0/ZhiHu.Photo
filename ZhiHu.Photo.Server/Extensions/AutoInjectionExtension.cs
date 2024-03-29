﻿using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ZhiHu.Photo.Server.Models.Attributes;

namespace ZhiHu.Photo.Server.Extensions
{
    public static class AutoInjectionExtension
    {
        public static void AddAutoServices(this IServiceCollection services, params string[] dllNames)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            if (dllNames.Length == 0)
            {
                var LocalPathDll = Assembly.GetExecutingAssembly().Location;
                dllNames = new[] { LocalPathDll };
            }

            var types = dllNames
                .Select(n => n.ToLower().EndsWith(".dll") ? n : $"{n}.dll")
                .Select(p => p.StartsWith(directory) ? p : $"{directory}{p}")
                .Where(File.Exists)
                .Select(p => AssemblyLoadContext.Default.LoadFromAssemblyPath(p))
                .SelectMany(a => a.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false, IsInterface: false } && t.Name.EndsWith("Service")))
                .Distinct();
            foreach (var t in types)
            {
                var _interface = t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}");
                if (_interface != null)
                {
                    RegistrationType(services, t, _interface);
                }
            }
        }

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
