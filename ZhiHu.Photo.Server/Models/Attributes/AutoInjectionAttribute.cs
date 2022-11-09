namespace ZhiHu.Photo.Server.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class AutoInjectionAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        public AutoInjectionAttribute()
        {

        }
        public AutoInjectionAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
