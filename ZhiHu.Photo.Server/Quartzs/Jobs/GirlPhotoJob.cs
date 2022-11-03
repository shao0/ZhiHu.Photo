using System.Diagnostics;
using AutoMapper;
using Newtonsoft.Json;
using Quartz;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.ZhiHu;
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
            Console.WriteLine("开始执行");
            try
            {
                await _scan.ScanInsert();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("执行结束");
        }


    }
}
