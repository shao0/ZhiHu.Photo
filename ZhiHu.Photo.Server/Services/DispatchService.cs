using Quartz;
using Quartz.Spi;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Quartzs.Jobs;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Singleton)]
    [PersistJobDataAfterExecution]
    public class DispatchService : IDispatchService
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly IConfiguration _config;
        private readonly IJobFactory _jobFactory;

        public DispatchService(ISchedulerFactory scheduler, IConfiguration config, IJobFactory jobFactory)
        {
            _scheduler = scheduler;
            _config = config;
            _jobFactory = jobFactory;
        }
        public async Task Start()
        {
            var scheduler = await _scheduler.GetScheduler();
            scheduler.JobFactory = _jobFactory;
            var job = JobBuilder.Create<GirlPhotoJob>()
                .WithIdentity(new JobKey("GirlPhoto"))
                .Build();
            var girlPhotoString = _config.GetSection("JobConfiguration:GirlPhoto")?.Value;
            var trigger = TriggerBuilder
                 .Create()
                 .StartNow()
                 .WithCronSchedule(girlPhotoString!)
                 .Build();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
