﻿using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Data;
using ZhiHu.Photo.Common.Dtos;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Controls;
using ZhiHu.Photo.Extensions;
using ZhiHu.Photo.Messages;
using ZhiHu.Photo.Models;
using ZhiHu.Photo.Services.Contacts;
using InfoType = ZhiHu.Photo.Models.InfoType;

namespace ZhiHu.Photo.ViewModels
{
    [ObservableObject]
    public partial class ShellViewModel
    {
        private readonly IAnswerService _answer;

        public ShellViewModel(IAnswerService answer)
        {
            _answer = answer;
        }

        private int PageSize = 20;
        /// <summary>
        /// 选择的详细信息
        /// </summary>
        private Information _ShownInfo { get; set; }
        /// <summary>
        /// 视频播放控件
        /// </summary>
        private PlayerControl _Player;
        /// <summary>
        /// 视频播放控件
        /// </summary>
        private PlayerControl Player
        {
            get { return _Player ??= new PlayerControl(); }
        }

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
        [ObservableProperty] private string? _json;
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
            var result = await _answer.GetAnswerList(PageIndex, PageSize);
            if (result.Status)
            {
                foreach (var answer in result.Data!.Items)
                {
                    var answerInfo = answer;
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
                                if (answerInfo.Images[i].Url.ToLower().EndsWith(".jpg") || answerInfo.Images[i].Url.ToLower().EndsWith(".jpeg"))
                                {
                                    if (answerInfo.Images[i].Video != null)
                                    {
                                        info.InfoType = InfoType.Video;
                                        if (!string.IsNullOrWhiteSpace(answerInfo.Images[i].Video?.HUrl))
                                        {
                                            info.Url = answerInfo.Images[i].Video.HUrl;
                                        }
                                        else if (!string.IsNullOrWhiteSpace(answerInfo.Images[i].Video?.SUrl))
                                        {
                                            info.Url = answerInfo.Images[i].Video.SUrl;
                                        }
                                        else if (!string.IsNullOrWhiteSpace(answerInfo.Images[i].Video?.LUrl))
                                        {
                                            info.Url = answerInfo.Images[i].Video.LUrl;
                                        }
                                    }
                                    else
                                    {
                                        info.InfoType = InfoType.Image;
                                    }
                                }
                                else if (answerInfo.Images[i].Url.ToLower().EndsWith(".gif"))
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
            if (info == _ShownInfo) return;
            switch (ImageSource)
            {
                case GifImage gif:
                    gif.Dispose();
                    break;
                case PlayerControl player:
                    player.StopPlay().GetAwaiter();
                    break;
            }

            if (info.InfoType != InfoType.Text && info.Bytes != null)
            {
                switch (info.InfoType)
                {
                    case InfoType.Image:
                        ImageSource = new Border { Background = new ImageBrush(info.Bytes.ConvertBitmapImage()) };
                        break;
                    case InfoType.Gif:
                        ImageSource = new GifImage(new MemoryStream(info.Bytes));
                        break;
                    case InfoType.Video:
                        if (!string.IsNullOrWhiteSpace(info.Url))
                        {
                            Player.Url = info.Url;
                            ImageSource = Player;
                        }
                        else
                        {
                            ImageSource = new Border { Background = new ImageBrush(info.Bytes.ConvertBitmapImage()) };
                        }
                        break;
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