namespace ZhiHu.Photo.Server.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class AutoInjectionAttribute : Attribute
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        /// <summary>
        /// 忽略
        /// </summary>
        public bool Ignore { get; set; }

        public AutoInjectionAttribute()
        {

        }
        /// <summary>
        /// 设置服务自动注入生命周期,是否忽略
        /// </summary>
        /// <param name="lifetime">生命周期</param>
        /// <param name="ignore">是否忽略</param>
        public AutoInjectionAttribute(ServiceLifetime lifetime, bool ignore = false)
        {
            Lifetime = lifetime;
            Ignore = ignore;
        }
    }
}
