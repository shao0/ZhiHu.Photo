using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Tools;
using ZhiHu.Photo.Messages;
using ScrollViewer = System.Windows.Controls.ScrollViewer;

namespace ZhiHu.Photo.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
            Loaded += ShellView_Loaded;
        }

        private async void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ShellView_Loaded;
            WeakReferenceMessenger.Default.Register<string, string>(this, MessageHelper.ShellViewScrollToTop, (obj, message) =>
            {
                if (message == nameof(ShellView))
                {
                    var scrollViewer = VisualHelper.GetChild<ScrollViewer>(Answer);
                    scrollViewer.ScrollToTop();
                }
            });
            //var url = "https://picx.zhimg.com/50/v2-75dcd52c8ebf92ee5c6cf866f794b2c7_720w.gif?source=1940ef5c";
            ////var url = "https://pica.zhimg.com/v2-bd931ca044171d0450ccccfbf2beca31_l.jpg?source=1940ef5c";

            //var httpClient = new HttpClient();
            //var bytes = await httpClient.GetByteArrayAsync(url);
            //Test.Child = bytes.ConvertBitmapImage();
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            var scrollViewer = VisualHelper.GetChild<ScrollViewer>(Answer);
            scrollViewer.RaiseEvent(eventArg);
        }


        private void ShellView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentBox.Height = MainContentBox.ActualHeight - 5;
            ContentBox.Width = MainContentBox.ActualWidth - 5;
        }

    }
}
