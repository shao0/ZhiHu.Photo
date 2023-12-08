using System.Text.RegularExpressions;
using ZhiHu.Photo.Server.Entities;
using ZhiHu.Photo.Server.Models.Attributes;
using ZhiHu.Photo.Server.Models.ZhiHu;
using ZhiHu.Photo.Server.Services.Interfaces;
using System.Text;
using System.Text.Json.Nodes;
using ZhiHu.Photo.Common.Extensions;
using ZhiHu.Photo.Server.Extensions;
using ZhiHu.Photo.Server.Helpers;

namespace ZhiHu.Photo.Server.Services
{
    [AutoInjection(ServiceLifetime.Singleton)]
    public class ScanService : IScanService
    {
        private readonly IConfiguration _config;
        private readonly Regex _regex;
        private readonly Regex _regexVideo;
        private readonly string _videoApiUrl;
        private readonly string _host;
        private IAnswerService _answer;
        private IImageService _image;
        /// <summary>
        /// 问题Id
        /// </summary>
        private string QuestionId { get; set; }
        /// <summary>
        /// 初始Url地址
        /// </summary>
        private string InitialUrl { get; set; }

        public ScanService(IConfiguration config,IImageService image,IAnswerService answer)
        {
            _config = config;
            _image = image;
            _answer = answer;
            _host = "www.zhihu.com";
            _regex = new Regex("img src=\"([\\s\\S]*?)\\?source=([\\s\\S]*?)\"");
            _regexVideo = new Regex("data-poster=\"([\\s\\S]*?)\" data-lens-id=\"([\\s\\S]*?)\">");
            _videoApiUrl = "https://lens.zhihu.com/api/v4/videos/";
            QuestionId = _config.GetSection("ZhiHuApi:QuestionId").Value;
            InitialUrl = $"https://www.zhihu.com/question/{QuestionId}/answers/updated";
        }

        public async void ScanInsert()
        {
            try
            {
                var url = await FirstGetUrlAsync();
                var all = _answer.FindAllAsync(_ => true);
                var max = all.Any() ? all.Max(e => e.AnswerUpdatedTimeStamp) : 0;
                await InsertAnswerAsync(url, true, max);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async void LastScanInsert()
        {
            try
            {
                var all = _answer.FindAllAsync(e => true);
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
                if (sleep > 3)
                {
                    await Task.Delay(1000);
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
                            entity.CreateDate = entity.UpdateDate = DateTime.Now;
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
                                var matchesVideos = _regexVideo.Matches(entity.Content);
                                foreach (Match matchesVideo in matchesVideos)
                                {
                                    var image = matchesVideo.Result("$1");
                                    var id = matchesVideo.Result("$2");
                                    if (string.IsNullOrWhiteSpace(image))
                                    {
                                        break;
                                    }
                                    var video = await GetVideoByVideoIdAsync(id);
                                    images.Add(new ImageEntity { Url = image, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Video = video });
                                }
                                entity.Images = images;
                            }
                            answers.Add(entity);
                        }

                        await _answer.UpdateAsync(answers);
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
        /// 获取视频通过视频Id
        /// </summary>
        /// <returns></returns>
        private async Task<VideoEntity> GetVideoByVideoIdAsync(string id)
        {
            var video = new VideoEntity();
            video.CreateDate = video.UpdateDate = DateTime.Now;
            var json = await WebHelper.GetJsonAsync($"{_videoApiUrl}{id}");
            var jObject = JsonNode.Parse(json).AsObject();
            if (jObject.ContainsKey("playlist"))
            {
                if (jObject["playlist"]["SD"] != null)
                {
                    video.SUrl = jObject["playlist"]["SD"]["play_url"].ToString();
                }
                if (jObject["playlist"]["LD"] != null)
                {

                    video.LUrl = jObject["playlist"]["LD"]["play_url"].ToString();
                }
                if (jObject["playlist"]["HD"] != null)
                {
                    video.HUrl = jObject["playlist"]["HD"]["play_url"].ToString();
                }
            }

            return video;
        }
        /// <summary>
        /// 通过问题id获取回答url
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetUrlByQuestionIdAsync()
        {
            var result = string.Empty;

            var html = await WebHelper.GetJsonAsync(InitialUrl, _host, InitialUrl);

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
                var jObject = JsonNode.Parse(sb.ToString());
                result = jObject["data"]["question"]["updatedAnswers"][QuestionId]["next"].ToString();
            }

            return result;
        }
        /// <summary>
        /// 异步获取知乎信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private async Task<ZhiHuInfo?> GetZhiHuInfoAsync(string? url, int number = 0)
        {
            if (number >= 5)
            {
                return null;
            }
            var json = string.Empty;
            ZhiHuInfo? info = null;
            try
            {
                json = await WebHelper.GetJsonAsync(url, _host, InitialUrl);
                info = json.ToObject<ZhiHuInfo>();
                info.Json = json;
            }
            catch (Exception e)
            {
                await Task.Delay(3000);
                number++;
                return await GetZhiHuInfoAsync(url, number);
                //Console.WriteLine(e);
                //var logPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Log";
                //if (!Directory.Exists(logPath))
                //{
                //    Directory.CreateDirectory(logPath);
                //}
                //await using var sw = File.CreateText($"{logPath}/Error{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json");
                //await sw.WriteAsync($"{e.Message}\r\n=======================================================\r\n{json}");
            }

            return info;
        }

        public async void LocalRefreshImage()
        {
            try
            {
                await _image.ClearTableAsync();
                var i = 0;
                var size = 1000;
                while (true)
                {
                    var images = new List<ImageEntity>();
                    var answers = _answer.FindAllAsync(_ => true).Skip(i * size).Take(size).ToArray();
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

                    await _image.InsertAsync(images);
                    i++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                    var answers = _answer.FindAllAsync(a => !a.Images.Any()).Skip(i * size).Take(size).ToArray();
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

                    await _image.InsertAsync(images);
                    i++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
