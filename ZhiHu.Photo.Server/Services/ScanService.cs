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
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Services.Interfaces.Bases;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Singleton)]
    public class ScanService : IScanService
    {
        private readonly IConfiguration _config;
        private readonly Regex _regex;
        private IAnswerService? _AnswerService;
        private IAnswerService AnswerService => _AnswerService ?? this.GetService<IAnswerService>();

        private IImageService? _imageService;
        private IImageService ImageService => _imageService ?? this.GetService<IImageService>();
        /// <summary>
        /// 问题Id
        /// </summary>
        private string QuestionId { get; set; }
        /// <summary>
        /// 初始Url地址
        /// </summary>
        private string InitialUrl { get; set; }

        public ScanService(IConfiguration config)
        {
            _config = config;
            _regex = new Regex("img src=\"([\\s\\S]*?)\\?source=([\\s\\S]*?)\"");
            QuestionId = _config.GetSection("ZhiHuApi:QuestionId").Value;
            InitialUrl = $"https://www.zhihu.com/question/{QuestionId}/answers/updated";
        }

        public async void ScanInsert()
        {
            try
            {
                var url = await FirstGetUrlAsync();
                var all = AnswerService.FindAllAsync(_ => true);
                var max = all.Any() ? all.Max(e => e.AnswerUpdatedTimeStamp) : 0;
                await InsertAnswerAsync(url, true, max);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                ScopeDispose?.Invoke();
            }
        }

        public async void LastScanInsert()
        {
            try
            {
                var all = AnswerService.FindAllAsync(e => true);
                if (all.Any())
                {
                    var entity = all.OrderBy(i => i.Id).Last();
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
                throw;
            }
            finally
            {
                ScopeDispose?.Invoke();
            }
        }
        /// <summary>
        /// 异步插入回答
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isScan"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private async Task InsertAnswerAsync(string url, bool isScan, int timeStamp)
        {
            var sleep = 0;
            while (true)
            {
                if (sleep > 10)
                {
                    await Task.Delay(3000);
                    sleep = 0;
                }
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

                        await AnswerService.UpdateAsync(answers);
                        if (!info.NextPage.IsEnd && !string.IsNullOrWhiteSpace(info.NextPage.NextUrl))
                        {
                            url = info.NextPage.NextUrl;
                            sleep++;
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
        /// <summary>
        /// 异步获取第一次地址
        /// </summary>
        /// <returns></returns>
        private async Task<string> FirstGetUrlAsync()
        {
            var zhiHu = _config.GetSection("ZhiHuApi");
            var key = zhiHu["Mode"];
            string url;
            if (key == "QuestionId")
            {
                url = await GetUrlByQuestionIdAsync();
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
        async Task<string> GetUrlByQuestionIdAsync()
        {
            var result = string.Empty;

            var client = new HttpClient();
            var html = await client.GetStringAsync(InitialUrl);

            var regex = new Regex(" type=\"text/json\">{\"initialState\":([\\s\\S]*?)}</script>");
            var matches = regex.Matches(html);
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
        /// <summary>
        /// 异步获取知乎信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<ZhiHuInfo?> GetZhiHuInfoAsync(string? url)
        {
            var json = string.Empty;
            ZhiHuInfo? info = null;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("referer", InitialUrl);
                json = await client.GetStringAsync(url);
                info = JsonConvert.DeserializeObject<ZhiHuInfo>(json);
                info.Json = json;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Log";
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                await using var sw = File.CreateText($"{logPath}/Error{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json");
                await sw.WriteAsync($"{e.Message}\r\n=======================================================\r\n{json}");
            }

            return info;
        }

        public async void LocalRefreshImage()
        {
            try
            {
                await ImageService.ClearTableAsync();
                var i = 0;
                var size = 1000;
                while (true)
                {
                    var images = new List<ImageEntity>();
                    var answers = AnswerService.FindAllAsync(_ => true).Skip(i * size).Take(size).ToArray();
                    if (!answers.Any()) return;
                    foreach (var entity in answers)
                    {
                        GetImageUrl(entity);
                        if (!entity.Images!.Any())
                        {
                            continue;
                        }
                        images.AddRange(entity.Images);
                    }

                    await ImageService.InsertAsync(images);
                    i++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                ScopeDispose?.Invoke();
            }

        }
        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="entity"></param>
        private void GetImageUrl(AnswerEntity entity)
        {
            List<ImageEntity> images = new();
            if (!string.IsNullOrWhiteSpace(entity.Content))
            {
                var matches = _regex.Matches(entity.Content);
                images.AddRange(matches
                    .Select(match => match.Result("$1"))
                    .Where(result => result.StartsWith("https://"))
                    .Distinct()
                    .Select(result => new ImageEntity
                    {
                        AnswerId = entity.Id,
                        Url = result,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    }));
            }
            entity.Images = images;
        }

        public async void ImagePatching()
        {
            try
            {
                var i = 0;
                var size = 1000;
                while (true)
                {
                    var images = new List<ImageEntity>();
                    var answers = AnswerService.FindAllAsync(a => !a.Images.Any()).Skip(i * size).Take(size).ToArray();
                    if (!answers.Any()) return;
                    foreach (var entity in answers)
                    {
                        GetImageUrl(entity);
                        if (!entity.Images!.Any())
                        {
                            continue;
                        }
                        images.AddRange(entity.Images);
                    }

                    await ImageService.InsertAsync(images);
                    i++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                ScopeDispose?.Invoke();
            }
        }
        public Action ScopeDispose { get; set; }
    }
}
