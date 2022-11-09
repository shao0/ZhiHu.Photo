using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Models.ZhiHu;
using ZhiHu.Photo.Server.Services.Bases;
using ZhiHu.Photo.Server.Services.Interfaces;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Scoped)]
    public class ScanService : BaseService<AnswerEntity>, IScanService
    {
        private readonly IConfiguration _config;

        public ScanService(IUnitOfWork work, IConfiguration config) : base(work)
        {
            _config = config;
        }
        public async Task ScanInsert()
        {
            try
            {
                var url = await FirstGetUrlAsync();
                var all = FindAllAsync(e => true);
                var max = all.Any() ? all.Max(e => e.AnswerUpdatedTimeStamp) : 0;
                await InsertAnswerAsync(url, true, max);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public async Task LastScanInsert()
        {
            try
            {
                var url = await FirstGetUrlAsync();
                var all = FindAllAsync(e => true);
                if (all.Any())
                {
                    var entity = all.Last();
                    await InsertAnswerAsync(entity.NextPageUrl!, false, entity.AnswerUpdatedTimeStamp);
                }
                else
                {
                    throw new Exception("无最后值");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
        private async Task InsertAnswerAsync(string url, bool isScan, int timeStamp)
        {
            while (true)
            {
                try
                {
                    var zz = "img src=\"([\\s\\S]*?)\"";
                    var regex = new Regex(zz);
                    var info = await GetZhiHuInfoAsync(url);
                    if (info != null)
                    {
                        var answers = new List<AnswerEntity>();
                        foreach (var data in info.Datas)
                        {
                            var entity = (AnswerEntity) data;
                            if (isScan)
                            {
                                if (timeStamp > 0 && entity.AnswerUpdatedTimeStamp <= timeStamp) continue;
                            }
                            else
                            {
                                if (entity.AnswerUpdatedTimeStamp > timeStamp) continue;

                            }
                            entity.Json = info.Json;
                            entity.NextPageUrl = info.NextPage.NextUrl;
                            var images = new List<ImageEntity>();
                            if (!string.IsNullOrWhiteSpace(entity.Content))
                            {
                                var matches = regex.Matches(entity.Content);
                                foreach (Match match in matches)
                                {
                                    var result = match.Result("$1");
                                    if (result.StartsWith("https://"))
                                    {
                                        images.Add(new ImageEntity { Url = result, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
                                    }
                                }

                                entity.Images = images;
                            }
                            answers.Add(entity);
                        }

                        await InsertAsync(answers);
                        if (!info.NextPage.IsEnd && !string.IsNullOrWhiteSpace(info.NextPage.NextUrl))
                        {
                            url = info.NextPage.NextUrl;
                            continue;
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                break;
            }
        }

        private async Task<string> FirstGetUrlAsync()
        {
            var url = _config.GetSection("ZhiHuApi:Photo")?.Value;
            var info = await GetZhiHuInfoAsync(url);
            if (info?.NextPage.Page == 0)
            {
                url = info.NextPage.NextUrl;
            }

            return url;
        }

        private static async Task<ZhiHuInfo?> GetZhiHuInfoAsync(string? url)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            var info = JsonConvert.DeserializeObject<ZhiHuInfo>(json);
            info.Json = json;
            return info;
        }

    }
}
