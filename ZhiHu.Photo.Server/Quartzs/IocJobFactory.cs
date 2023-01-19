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
        private ConcurrentDictionary<string, IServiceScope> scopes { get; set; } = new();

        public IocJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var serviceScope = scopes.ContainsKey(bundle.JobDetail.JobType.FullName) ? scopes[bundle.JobDetail.JobType.FullName] : _serviceProvider.CreateScope();
            return serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
            if (scopes.ContainsKey(job.GetType().FullName)) scopes[job.GetType().FullName].Dispose();
        }
    }
}
