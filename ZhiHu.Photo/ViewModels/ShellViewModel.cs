using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Newtonsoft.Json;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Extensions;
using ZhiHu.Photo.Messages;
using ZhiHu.Photo.Models;
using InfoType = ZhiHu.Photo.Models.InfoType;

namespace ZhiHu.Photo.ViewModels
{
    [ObservableObject]
    public partial class ShellViewModel
    {
        private static string Url = ConfigurationManager.AppSettings["Url"];
        private int PageSize = 20;

        /// <summary>
        /// 选择查看的详细信息
        /// </summary>
        private Information? SelectViewInfo { get; set; }
        /// <summary>
        /// 展示的回答
        /// </summary>
        public ObservableCollection<AnswerInfo> Answers { get; set; } = new();
        /// <summary>
        /// 展示容器宽
        /// </summary>
        public double W { get; set; }
        /// <summary>
        /// 展示容器高
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// json
        /// </summary>
        [ObservableProperty] private string _json;
        /// <summary>
        /// 最大页数
        /// </summary>

        [ObservableProperty] private int _pageMax = 1;
        /// <summary>
        /// 当前页
        /// </summary>

        [ObservableProperty] private int _pageIndex = 1;
        /// <summary>
        /// 选择展示控件
        /// </summary>

        [ObservableProperty] private FrameworkElement? _imageSource;

        /// <summary>
        /// 宽
        /// </summary>
        [ObservableProperty] private double _width;

        /// <summary>
        /// 高
        /// </summary>
        [ObservableProperty] private double _height;

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Query()
        {
            WeakReferenceMessenger.Default.Send("ShellView", MessageHelper.ShellViewScrollToTop);
            Answers.Clear();
            ImageSource = null;
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
                            Information info;
                            var s = strings[i];
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                info = new Information();
                                info.InfoType = InfoType.Text;
                                info.Content = s;
                                answerInfo.InfoList.Add(info);
                            }
                            if (answerInfo.Images != null && answerInfo.Images.Length > i)
                            {
                                info = new Information();
                                if (answerInfo.Images[i].Url.ToLower().EndsWith(".jpg?source=1940ef5c"))
                                {
                                    info.InfoType = InfoType.Image;
                                }
                                else if (answerInfo.Images[i].Url.ToLower().EndsWith(".gif?source=1940ef5c"))
                                {
                                    info.InfoType = InfoType.Gif;
                                }
                                info.Content = answerInfo.Images[i].Url;
                                answerInfo.InfoList.Add(info);
                            }
                        }
                    }
                    Answers.Add(answerInfo);
                }

                PageMax = result.Data.TotalPages;
            }
            else
            {
                Growl.Warning(result.Message);
            }
        }
        /// <summary>
        /// 更新页
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task PageUpdated(FunctionEventArgs<int> info)
        {
            PageIndex = info.Info;
            await Query();
        }
        /// <summary>
        /// 选择详细信息
        /// </summary>
        /// <param name="info"></param>
        [RelayCommand]
        void SelectedInfo(Information info)
        {
            if (ImageSource is GifImage gif)
            {
                gif.Dispose();
            }

            if (info.InfoType != InfoType.Text && info.Bytes != null)
            {
                if (info.InfoType == InfoType.Image)
                {
                    ImageSource = new Border { Background = new ImageBrush(info.Bytes.ConvertBitmapImage()) };
                }
                else
                {
                    ImageSource = new GifImage(new MemoryStream(info.Bytes));
                }
                var ratio = info.Width / info.Height;
                var width = H * ratio;
                var height = W / ratio;
                if (ratio > 1)
                {
                    if (width < W)
                    {
                        Height = H;
                        Width = width;
                    }
                    else
                    {
                        Height = height;
                        Width = W;
                    }
                }
                else if (ratio < 1)
                {
                    if (height < H)
                    {
                        Height = height;
                        Width = W;
                    }
                    else
                    {

                        Height = H;
                        Width = width;
                    }
                }
                else
                {
                    if (W > H)
                    {
                        Width = Height = H;
                    }
                    else
                    {
                        Width = Height = W;

                    }
                }

            }

            SelectViewInfo = info;
        }

    }
}