using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZhiHu.Photo.Server.DatabaseContext.UnitOfWork;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Models.ZhiHu;
using ZhiHu.Photo.Server.Services.Bases;
using ZhiHu.Photo.Server.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Scoped)]
    public class ScanService : BaseService<AnswerEntity>, IScanService
    {
        private readonly IConfiguration _config;
        Regex _regex = new Regex("img src=\"([\\s\\S]*?)\\?source=1940ef5c\"");

        string QuestionId { get; set; }
        string InitialUrl { get; set; }

        public ScanService(IUnitOfWork work, IConfiguration config) : base(work)
        {
            _config = config;
            QuestionId = _config.GetSection("ZhiHuApi:QuestionId").Value;
            InitialUrl = $"https://www.zhihu.com/question/{QuestionId}/answers/updated";
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
                var all = FindAllAsync(e => true);
                if (all.Any())
                {
                    var entity = all.OrderBy(i=> i.Id).Last();
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
                                var matches = _regex.Matches(entity.Content);
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
            var zhiHu = _config.GetSection("ZhiHuApi");
            var key = zhiHu["Mode"];
            string url;
            if (key == "QuestionId")
            {
                url = await GetUrlByQuestionId();
            }
            else
            {
                url = zhiHu.GetSection("PhotoUrl").Value;
                var info = await GetZhiHuInfoAsync(url);
                if (info?.NextPage.Page == 0)
                {
                    url = info.NextPage.NextUrl;
                }
            }


            return url;
        }
        /// <summary>
        /// 通过问题id获取回答url
        /// </summary>
        /// <returns></returns>
        async Task<string> GetUrlByQuestionId()
        {
            var result = string.Empty;

            var client = new HttpClient();
            var html = await client.GetStringAsync(InitialUrl);

            var _regex = new Regex(" type=\"text/json\">{\"initialState\":([\\s\\S]*?)}</script>");
            var matches = _regex.Matches(html);
            var first = matches.FirstOrDefault();
            if (first != null)
            {
                var json = first.Result("$1");
                var sb = new StringBuilder();
                sb.Append("{\"data\":");
                sb.Append(json);
                sb.Append('}');
                var jObject = JObject.Parse(sb.ToString());
                result = jObject["data"]["question"]["updatedAnswers"][QuestionId]["next"].ToString();
            }

            return result;
        }
        private async Task<ZhiHuInfo?> GetZhiHuInfoAsync(string? url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("referer", InitialUrl);
            var json = await client.GetStringAsync(url);
            var info = JsonConvert.DeserializeObject<ZhiHuInfo>(json);
            info.Json = json;
            return info;
        }

        public async Task LocalRefreshImage()
        {
            var answerRepository = _work.GetRepository<AnswerEntity>();
            var answerAll = answerRepository.GetAll();
            var images = new List<ImageEntity>();
            foreach (var entity in answerAll)
            {
                if (!string.IsNullOrWhiteSpace(entity.Content))
                {
                    images.Clear();
                    var matches = _regex.Matches(entity.Content);
                    images.AddRange(from Match match in matches
                                    let result = match.Result("$1")
                                    where result.StartsWith("https://")
                                    select new ImageEntity { AnswerId = entity.Id, Url = result, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
                    entity.Images = images;
                }
                await _work.SaveChangesAsync();
            }
            //var index = 1;
            //while (true)
            //{
            //    var entities = answerAll.Skip(index * 1).Take(1);
            //    if (!entities.Any()) break;
            //    var images = new List<ImageEntity>();
            //    foreach (var entity in entities)
            //    {
            //        if (!string.IsNullOrWhiteSpace(entity.Content))
            //        {
            //            var matches = _regex.Matches(entity.Content);
            //            foreach (Match match in matches)
            //            {
            //                var result = match.Result("$1");
            //                if (result.StartsWith("https://"))
            //                {
            //                    images.Add(new ImageEntity { AnswerId = entity.Id,Url = result, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
            //                }
            //            }
            //            entity.Images = images;
            //        }
            //    }
            //    await _work.SaveChangesAsync();
            //    index++;
            //}

        }
    }
}
