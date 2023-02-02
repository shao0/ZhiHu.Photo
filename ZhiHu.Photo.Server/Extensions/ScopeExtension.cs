using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Extensions
{
    public static class ScopeExtension
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        public static IServiceProvider Provider { get; set; }

        /// <summary>
        /// 范围控制获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static T GetService<T>(this IScopeControl control)
        {
            var scope = Provider.CreateScope();
            control.ScopeDispose = () => scope.Dispose();
            return scope.ServiceProvider.GetService<T>();
        }
        /// <summary>
        /// 范围控制获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static object GetService(this IScopeControl control, Type type)
        {
            var scope = Provider.CreateScope();
            control.ScopeDispose = () => scope.Dispose();
            return scope.ServiceProvider.GetService(type);
        }

    }
}
