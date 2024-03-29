﻿using Quartz;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Quartzs.Jobs
{
    public class GirlPhotoJob : IJob
    {
        private readonly IScanService _scan;

        public GirlPhotoJob(IScanService scan)
        {
            _scan = scan;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Yield();
            Console.WriteLine("开始执行");
            try
            {
                 _scan.ScanInsert();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("执行结束");
        }


    }
}
