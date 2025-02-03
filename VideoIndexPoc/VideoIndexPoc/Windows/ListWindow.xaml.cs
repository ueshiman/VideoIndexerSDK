using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VideoIndexPoc.Windows;
using VideoIndexPoc.VideoIndexerClient;
using VideoIndexPoc.VideoIndexerClient.ApiAccess;
using VideoIndexPoc.VideoIndexerClient.Auth;
using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.Windows
{
    public sealed partial class ListWindow : Window
    {
        private readonly IVideoListAccess _videoListAccess;
        private readonly IVideoIndexerClient _videoIndexerClient;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        public ObservableCollection<VideoListItem> Videos { get; set; }
        public MainWindow? _myMainWindow { get; set; } = null;

        public ListWindow(IVideoListAccess videoListAccess, IVideoIndexerClient videoIndexerClient, IAccountTokenProviderDynamic accountTokenProvider)
        //public ListWindow(IVideoListAccess videoListAccess, IVideoIndexerClient videoIndexerClient, IAccountTokenProvider accountTokenProvider)
        {
            InitializeComponent();

            // イベントハンドラを追加
            this.Closed += Window_Closed;

            _videoListAccess = videoListAccess;
            _videoIndexerClient = videoIndexerClient;
            _accountTokenProvider = accountTokenProvider;


            //_videoIndexerClient.AuthenticateAsync().Wait();

            //var armAccessToken = AccountTokenProvider.GetArmAccessToken();
            ////var armAccessToken = _accountTokenProvider.GetArmAccessTokenAsync().Result;
            //_videoIndexerClient.Authenticate();
            //var account = _videoIndexerClient.GetAccount(Constants.ViAccountName);

            //var accountAccessToken = _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken).Result;

            //var videoList = _videoListAccess.GetVideoList(account.Location, account.Properties.Id, accountAccessToken);

            //Videos = new ObservableCollection<VideoListItem>(videoList);

            // モックデータを追加 (実際はAzure APIを使用)
            Videos = new ObservableCollection<VideoListItem>
                {
                    new VideoListItem
                    {
                        name = "Sample Video 1",
                        durationInSeconds = 2,
                        description = "This is a sample video.",
                        thumbnailUrl = "https://example.com/thumbnail1.jpg"
                    },
                    new VideoListItem
                    {
                        name = "Sample Video 2",
                        durationInSeconds = 3,
                        description = "Another sample video.",
                        thumbnailUrl = "https://example.com/thumbnail2.jpg"
                    }
                };

            // データバインディング
            VideoListView.ItemsSource = Videos ?? new ObservableCollection<VideoListItem>();
        }

        // Windowsロード時のイベントハンドラ
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Add your code here to handle the window loaded event
        }

        private void Window_Closed(object sender, WindowEventArgs e)
        {
            _myMainWindow?.CloseListWindow();
            // Add your code here to handle the window closed event
            // 非表示にする
            // ウィンドウの閉じる動作をキャンセル
            //e.Handled = true;
            // ウィンドウを最小化する
            //this.Minimize();
        }

        private async void VideoListView_OnLoadedWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
            //var armAccessToken = _accountTokenProvider.GetArmAccessTokenAsync().Result;
            await _videoIndexerClient.AuthenticateAsync();
            var account = await _videoIndexerClient.GetAccountAsync(Constants.ViAccountName);

            var accountAccessToken = await _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken);

            var videoList = await _videoListAccess.GetVideoListAsync(account.Location, account.Properties.Id, accountAccessToken);

            Videos = new ObservableCollection<VideoListItem>(videoList);
            // データバインディング
            VideoListView.ItemsSource = Videos ?? new ObservableCollection<VideoListItem>();
        }

        private async void DataStackPanel_PointerPressedp(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            StackPanel? stackPanel = sender as StackPanel;
            var id = stackPanel?.Children.OfType<TextBlock>().FirstOrDefault(child => child.Name == "id")?.Text;
            if(string.IsNullOrEmpty(id)) return;
            // idから動画情報を取得
            var itemWindow = App.AppHost.Services.GetRequiredService<Windows.ItemWindow>();
            itemWindow._myMainWindow = _myMainWindow;
            await itemWindow.Load(id);
            itemWindow.Activate();
        }
    }

    public class VideoInfo
    {
        public string VideoName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}