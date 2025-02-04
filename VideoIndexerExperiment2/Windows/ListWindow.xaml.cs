using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccess.Repositories.VideoItemRepository;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerExperiment2.Windows
{
    public sealed partial class ListWindow : Window
    {
        private readonly IVideoListApiAccess _videoListAccess;
        //private readonly IVideoIndexerClient _videoIndexerClient;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IVideoListRepository _videoListRepository;
        private readonly IVideoListDataModelMapper _videoListDataModelMapper;

        public ObservableCollection<VideoListDataModel> Videos { get; set; }
        public MainWindow? MyMainWindow { get; set; } = null;

        //public ListWindow(IVideoListRepository videoListRepository)
        public ListWindow(IVideoListApiAccess videoListAccess, IAccountTokenProviderDynamic accountTokenProvider, IApiResourceConfigurations apiResourceConfigurations, IAccounApitAccess accountAccess, IVideoListDataModelMapper videoListDataModelMapper, IVideoListRepository videoListRepository)
        //public ListWindow(IVideoListAccess videoListAccess, IVideoIndexerClient videoIndexerClient, IAccountTokenProvider accountTokenProvider)
        {
            //_videoListRepository = videoListRepository;
            InitializeComponent();

            // イベントハンドラを追加
            this.Closed += Window_Closed;

            _videoListAccess = videoListAccess;
            //_videoIndexerClient = videoIndexerClient;
            _accountTokenProvider = accountTokenProvider;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountAccess = accountAccess;
            //_videoListRepository = videoListRepository;
            _videoListDataModelMapper = videoListDataModelMapper;
            _videoListRepository = videoListRepository;


            //_videoIndexerClient.GetAccessTkenAsync().Wait();

            //var armAccessToken = AccountTokenProvider.GetArmAccessToken();
            ////var armAccessToken = _accountTokenProvider.GetArmAccessTokenAsync().Result;
            //_videoIndexerClient.GetAccessToken();
            //var account = _videoIndexerClient.GetAccount(Constants.ViAccountName);

            //var accountAccessToken = _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken).Result;

            //var videoList = _videoListAccess.ListVideos(account.Location, account.Properties.Id, accountAccessToken);

            //Videos = new ObservableCollection<VideoListItem>(videoList);

            // モックデータを追加 (実際はAzure APIを使用)
            Videos = new ObservableCollection<VideoListDataModel>
                {
                    new VideoListDataModel
                    {
                        Name = "Sample Video 1",
                        DurationInSeconds = 2,
                        Description = "This is a sample video.",
                        ThumbnailUrl = "https://example.com/thumbnail1.jpg"
                    },
                    new VideoListDataModel
                    { 
                        Name = "Sample Video 2",
                        DurationInSeconds = 3,
                        Description = "Another sample video.",
                        ThumbnailUrl = "https://example.com/thumbnail2.jpg"
                    }
                };

            // データバインディング
            VideoListView.ItemsSource = Videos ?? new ObservableCollection<VideoListDataModel>();
        }

        // Windowsロード時のイベントハンドラ
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Add your code here to handle the window loaded event
        }

        private void Window_Closed(object sender, WindowEventArgs e)
        {
            MyMainWindow?.CloseListWindow();
            // Add your code here to handle the window closed event
            // 非表示にする
            // ウィンドウの閉じる動作をキャンセル
            //e.Handled = true;
            // ウィンドウを最小化する
            //this.Minimize();
        }

        private async void VideoListView_OnLoadedWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            //var armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
            //var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName);

            //var accountAccessToken = await _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken);

            //var videoApiList = await _videoListAccess.ListVideosAsync(account.Location, account.Properties.Id, accountAccessToken);

            //var videoList = videoApiList.Select(_videoListDataModelMapper.MapFrom);


            var videoList = await _videoListRepository.ListVideosAsync();

            Videos = new ObservableCollection<VideoListDataModel>(videoList);
            // データバインディング
            VideoListView.ItemsSource = Videos ?? new ObservableCollection<VideoListDataModel>();
        }

        private async void DataStackPanel_PointerPressedp(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            StackPanel? stackPanel = sender as StackPanel;
            var id = stackPanel?.Children.OfType<TextBlock>().FirstOrDefault(child => child.Name == "Id")?.Text;
            if(string.IsNullOrEmpty(id)) return;
            // idから動画情報を取得
            var itemWindow = App.AppHost.Services.GetRequiredService<ItemWindow>();
            itemWindow.MyMainWindow = MyMainWindow;
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