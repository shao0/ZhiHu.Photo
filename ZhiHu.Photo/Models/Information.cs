using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;
using ZhiHu.Photo.Extensions;

namespace ZhiHu.Photo.Models
{
    [ObservableObject]
    public partial class Information
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoType InfoType { get; set; }
        /// <summary>
        /// 内容数据
        /// </summary>
        public byte[]? Bytes { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public double Height { get; set; }

        private string _content = string.Empty;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Loaded();
                }
            }
        }


        /// <summary>
        /// 加载中
        /// </summary>
        [ObservableProperty] private bool _loading;
        /// <summary>
        /// 图片
        /// </summary>
        [ObservableProperty] private BitmapImage? _img;

        /// <summary>
        /// 宽
        /// </summary>
        [ObservableProperty] private double _w = 200;

        /// <summary>
        /// 高
        /// </summary>
        [ObservableProperty] private double _h = 100;


        async void Loaded()
        {
            try
            {
                if (InfoType != InfoType.Text)
                {
                    var httpClient = new HttpClient();
                    Bytes = await httpClient.GetByteArrayAsync(Content);
                    using (var ms = new MemoryStream(Bytes))
                    {
                        BitmapDecoder? decoder = InfoType switch
                        {
                            InfoType.Image => new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.Default),
                            InfoType.Gif => new GifBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.Default),
                            _ => null
                        };
                        if (decoder != null)
                        {
                            var frame = decoder.Frames[0];
                            Width = frame.PixelWidth;
                            Height = frame.PixelHeight;
                            H = W * Height / Width;
                        }
                    }
                    Img = Bytes.ConvertBitmapImage((int) (Width / 5));
                }
            }
            catch (Exception e)
            {
                Growl.Error($"{e.Message}\r\n{Content}");
            }
        }

    }
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum InfoType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 动图
        /// </summary>
        Gif,
    }
}
