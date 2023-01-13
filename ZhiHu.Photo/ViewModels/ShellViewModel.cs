using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Newtonsoft.Json;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Extensions;
using ZhiHu.Photo.Models;
using InfoType = ZhiHu.Photo.Models.InfoType;

namespace ZhiHu.Photo.ViewModels
{
    [ObservableObject]
    public partial class ShellViewModel
    {
        private static string Url = ConfigurationManager.AppSettings["Url"];
        private int PageSize = 10;
        public ObservableCollection<AnswerInfo> Answers { get; set; } = new();

        [ObservableProperty] private string _json;

        [ObservableProperty] private int _pageMax = 1;

        [ObservableProperty] private int _pageIndex = 1;

        [RelayCommand]
        private async Task Query()
        {
            Answers.Clear();
            var url = $"{Url}/api/Answer/GetAll?PageIndex={PageIndex - 1}&PageSize={PageSize}&Search=";
            var client = new HttpClient();
            Json = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<ApiResponse<PagedList<AnswerDto>>>(Json);
            if (result.Status)
            {
                foreach (var answer in result.Data!.Items)
                {
                    var answerInfo = answer.Map<AnswerDto, AnswerInfo>();
                    if (!string.IsNullOrWhiteSpace(answerInfo.Excerpt))
                    {
                        var strings = answerInfo.Excerpt.Split("[图片]");
                        for (var i = 0; i < strings.Length; i++)
                        {
                            var s = strings[i];
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                var info = new Information();
                                info.InfoType = InfoType.Text;
                                info.Content = s;
                            }
                            if (answerInfo.Images != null && answerInfo.Images.Length > i)
                            {
                                var info = new Information();
                                info.InfoType = InfoType.Image;
                                info.Content = answerInfo.Images[i].Url;
                            }
                        }
                    }
                    Answers.Add(answerInfo);
                }

                PageMax = result.Data.TotalPages;
            }
            else
            {

            }
        }

        [RelayCommand]
        private async Task PageUpdated(FunctionEventArgs<int> info)
        {
            PageIndex = info.Info;
            await Query();
        }
    }
}