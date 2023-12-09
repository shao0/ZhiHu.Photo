using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ZhiHu.Photo.Helpers
{
    public static class AssemblyHelper
    {
        private static Dictionary<string, Type[]> TypeCache { get; } = new();
        /// <summary>
        /// 获取程序集下所有类型
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyTypes(string dllName)
        {
            IEnumerable<Type> types = Type.EmptyTypes;
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            dllName = dllName.ToLower().EndsWith(".dll") ? dllName : $"{dllName}.dll";
            if (!TypeCache.TryGetValue(dllName, out var ts))
            {
                var path = dllName.StartsWith(directory) ? dllName : $"{directory}{dllName}";
                if (!File.Exists(path)) return types;
                types = AssemblyLoadContext.Default.LoadFromAssemblyPath(path).GetTypes().Distinct();
                TypeCache.Add(dllName, types.ToArray());
            }
            else
            {
                types = ts;
            }

            return types;
        }
        /// <summary>
        /// 获取程序集下所有类型
        /// </summary>
        /// <param name="dllNames"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyTypes( params string[]? dllNames )
        {
            if (dllNames == null || dllNames.Length == 0)
            {
                var localPathDll = Assembly.GetExecutingAssembly().Location;
                dllNames = new[] { localPathDll };
            }
            return dllNames.SelectMany(GetAssemblyTypes);
        }
    }
}
