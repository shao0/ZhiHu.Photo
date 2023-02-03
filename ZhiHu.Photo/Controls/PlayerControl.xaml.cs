using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;

namespace ZhiHu.Photo.Controls
{
    /// <summary>
    /// PlayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerControl : UserControl
    {

        #region string FilePath 文件地址

        /// <summary>
        /// 文件地址 依赖属性获取设置值
        /// </summary>
        public string FilePath
        {
            get => (string) GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }
        /// <summary>
        /// 文件地址 依赖属性
        /// </summary>
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
            nameof(FilePath), typeof(string), typeof(PlayerControl), new PropertyMetadata(default(string), FilePathChanged));

        private static void FilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlayerControl player)
            {
                player.StartPlay(PlayMode.File);
            }
        }

        #endregion


        #region string Web 网络地址

        /// <summary>
        /// 网络地址 依赖属性获取设置值
        /// </summary>
        public string Url
        {
            get => (string) GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }
        /// <summary>
        /// 网络地址 依赖属性
        /// </summary>
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
            nameof(Url), typeof(string), typeof(PlayerControl), new PropertyMetadata(default(string), UrlChanged));

        private static void UrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlayerControl player)
            {
                player.StartPlay(PlayMode.Web);
            }
        }

        #endregion


        public PlayerControl()
        {
            InitializeComponent();
            Initial();
        }
        private void Initial()
        {
            //创建播放器
            var appPath = AppDomain.CurrentDomain.BaseDirectory; //获取输出目录
            //根据系统32/64选择文件夹，否则会报错 不是有效的 Win32 应用程序。
            //IntPtr.Size == 4 表示当前程序是32位 x86的 
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(appPath, "VLC"));//vlc文件的地址
            //配置项
            string[] options = {
                //添加日志
                //   "--file-logging", "-vvv", "--logfile=Logs.log"
                "--network-caching=300",//尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
            };
            //创建播放器
            Vlc.SourceProvider.CreatePlayer(vlcLibDirectory, options);
            //添加播放事件
            Vlc.SourceProvider.MediaPlayer.EndReached += MediaPlayer_EndReached;//播放结束
            Vlc.SourceProvider.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;//播放位置改变事件-刷新播放进度
            Vlc.SourceProvider.MediaPlayer.LengthChanged += MediaPlayer_LengthChanged;//获取播放总时长
            Vlc.SourceProvider.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;//获取播放当前时间
            Vlc.SourceProvider.MediaPlayer.Rate = 1;
        }

        private void MediaPlayer_TimeChanged(object? sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
        }

        private void MediaPlayer_LengthChanged(object? sender, VlcMediaPlayerLengthChangedEventArgs e)
        {
        }

        private void MediaPlayer_PositionChanged(object? sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
        }

        private void MediaPlayer_EndReached(object? sender, VlcMediaPlayerEndReachedEventArgs e)
        {
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <returns></returns>
        public async Task StopPlay()
        {
            await Task.Run(() =>
            {
                Vlc.SourceProvider.MediaPlayer?.Stop();
            });
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <returns></returns>
        public async Task PausePlay()
        {
            await Task.Run(() =>
            {
                Vlc.SourceProvider.MediaPlayer?.Pause();
            });
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <returns></returns>
        public async Task Play()
        {
            await Task.Run(() =>
            {
                Vlc.SourceProvider.MediaPlayer?.Play();
            });
        }
        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="mode"></param>
        public async void StartPlay(PlayMode mode)
        {
            await StopPlay();
            try
            {
                var ulr = Url;
                var (result, msg) = await Task.Run(() =>
                {
                    bool state;
                    var message = "播放完成";
                    switch (mode)
                    {
                        default:
                        case PlayMode.File:
                            if (!File.Exists(FilePath))
                            {
                                message = $"播放文件不存在:{FilePath}";
                            }
                            Vlc.SourceProvider.MediaPlayer.Play(FilePath);
                            state = true;
                            break;
                        case PlayMode.Web:
                            Vlc.SourceProvider.MediaPlayer.Play(new Uri(ulr));
                            state = true;
                            break;
                    }
                    return (status: state, msg: message);
                });
                if (!result)
                {
                    Growl.Warning(msg);
                }
                else
                {
                    Growl.Info(msg);
                }
            }
            catch (Exception e)
            {
                Growl.Error(e.Message);
            }
        }

    }
    /// <summary>
    /// 播放模式
    /// </summary>
    public enum PlayMode
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,
        /// <summary>
        /// 网络
        /// </summary>
        Web
    }
}
