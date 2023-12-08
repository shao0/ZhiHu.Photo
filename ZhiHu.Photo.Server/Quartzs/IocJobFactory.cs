using System.Collections.Concurrent;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace ZhiHu.Photo.Server.Quartzs
{
    public class IocJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, IServiceScope> Scopes { get; } = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public IocJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (!Scopes.TryGetValue(bundle.JobDetail.JobType.FullName, out var serviceScope))
            {
                serviceScope ??= _serviceProvider.CreateScope();
            }
            return serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var key = job.GetType().FullName;
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }

            if (!Scopes.ContainsKey(key)) return;
            Scopes[key].Dispose();
            Scopes.TryRemove(key, out _);

        }
    }
}
