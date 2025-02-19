using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.VideoItemRepository;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.Parser;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexerExperiment2.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemWindow : Window
    {
        private const string ParamName = "JsonSerializer.Deserialize<Account>(jsonResponseBody)";
        private readonly IVideoItemApiAccess _videoItemAccess;
        private readonly IVideoItemParser _videoItemParser;
        //private readonly IVideoIndexerClient _videoIndexerClient;
        private readonly IVideoDownloadApiAccess _videoDownload;
        public MainWindow? MyMainWindow { get; set; } = null;

        private readonly ILogger<ItemWindow> _logger;
        //private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        //private readonly IAuthenticator _authenticator;
        //private readonly IAccounApitAccess _accountAccess;
        private readonly IVideoIndexRepository _videoIndexRepository;
        private readonly IVideoDataRepository _videoDataRepository;

        public ObservableCollection<VideoItemDataModel> VideoItems { get; set; } = new ObservableCollection<VideoItemDataModel>();

        public ItemWindow(ILogger<ItemWindow> logger, IApiResourceConfigurations apiResourceConfigurations, IVideoItemApiAccess videoItemAccess, IVideoItemParser videoItemParser, IVideoDownloadApiAccess videoDownload, IVideoIndexRepository videoIndexRepository, IVideoDataRepository videoDataRepository)
        {
            _logger = logger;
            //_authenticator = authenticator;
            //_accountAccess = accountAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _videoItemAccess = videoItemAccess;
            _videoItemParser = videoItemParser;
            _videoDownload = videoDownload;
            _videoIndexRepository = videoIndexRepository;
            _videoDataRepository = videoDataRepository;
            this.InitializeComponent();
        }


        //private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        //{

        //    // モックデータを追加 (実際はAzure APIを使用)
        //    VideoItems =
        //        [
        //            new VideoItem()
        //            {
        //                summarizedInsights = new Summarizedinsights()
        //                {
        //                    faces = new Face[]
        //                    {
        //                        new Face
        //                        {
        //                            Name = "Person 1",
        //                            AppearanceCount = 2
        //                        },
        //                        new Face
        //                        {
        //                            Name = "Person 2",
        //                            AppearanceCount = 3
        //                        }
        //                    }
        //                }
        //            }
        //        ];

        //    // データバインド

        //}

        public async Task Load(string videoId)
        {
            //var accountAccessToken = await _authenticator.GetAccessTkenAsync();

            //var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ??  throw new ArgumentNullException(paramName: ParamName);

            VideoItemDataModel video = await _videoIndexRepository.GetVideoItemAsync(videoId);
            //var video = await _videoItemAccess.GetVideoItemAsync(account.Location, account.Properties.Id, videoId, accountAccessToken);
            VideoItems.Add(video);
            //VideoItems.Add(GetDummyItem());
        }

        private VideoItemDataModel GetDummyItem()
        {
            return new VideoItemDataModel()
            {
                SummarizedInsights = new Summarizedinsights()
                {
                    Faces = new Face[]
                    {
                        new Face
                        {
                            Name = "Person 1",
                            AppearanceCount = 2
                        },
                        new Face
                        {
                            Name = "Person 2",
                            AppearanceCount = 3
                        }
                    }
                }
            };
        }

        private async void OnDownloadButtonClick(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement element && element.DataContext is VideoItemDataModel videoItem)
            {
                var videoId = videoItem.Id;

                //var accountAccessToken = await _authenticator.GetAccessTkenAsync();
                //var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName);

                //var video = await _videoItemAccess.GetVideoItemAsync(account.Location, account.Properties.Id, videoId, accountAccessToken);
                var video = await _videoIndexRepository.GetVideoItemAsync(videoId ?? throw new InvalidOperationException());

                // ダウンロード処理をここに追加
                // ファイル保存ダイアログを作成
                var savePicker = new FileSavePicker();

                // WinUI 3 では Win32 環境で動作するため、ハンドルを設定する必要があります。
                //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MyMainWindow);
                // ファイルタイプとデフォルトファイル名を設定
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                //savePicker.SuggestedStartLocation = PickerLocationId.Downloads;
                savePicker.FileTypeChoices.Add("MP4 file", new[] { ".mp4" });
                savePicker.SuggestedFileName = "DownloadedVideo";
                savePicker.SettingsIdentifier = "settingsIdentifier";
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                //string downloadUrl = await _videoDownload.GetVideoDownloadUrl(account.Properties.Id, videoId, accountAccessToken, account.Location);
                //await DownloadVideoAsync(downloadUrl.Trim('"'), $"{DateTime.Now:yyyyMMddHHmmss}.mp4");


                try
                {
                    // 保存先をユーザーに選ばせる
                    StorageFile file = await savePicker.PickSaveFileAsync();

                    if (file != null)
                    {
                        // 例: ダウンロードしたビデオを保存
                        //string downloadUrlResponse = await _videoDownload.GetVideoDownloadUrl(account.Location, account.Properties.Id, videoId, accountAccessToken);
                        string downloadUrlResponse = await _videoDataRepository.GetVideoDownloadUrl(videoId);
                        string downloadUrl = downloadUrlResponse.Trim('"');
                        await DownloadVideoAsync(downloadUrl, file);

                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "完了",
                            Content = "ビデオが保存されました。",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };

                        await dialog.ShowAsync();
                    }
                    else
                    {
                        // ユーザーがキャンセルした場合
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "キャンセル",
                            Content = "保存がキャンセルされました。",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };

                        await dialog.ShowAsync();
                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }


            }
        }

        private async System.Threading.Tasks.Task DownloadVideoAsync(string url, string filePath)
        {
            try
            {
                // ダウンロード処理
                using var httpClient = new System.Net.Http.HttpClient();
                // HTTPリクエストをストリームで実行
                using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // ダウンロードされたストリームをローカルファイルに書き込む
                await using var inputStream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await inputStream.CopyToAsync(fileStream); // ストリームを直接コピー
                await fileStream.FlushAsync(); // バッファをフラッシュ
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        private async System.Threading.Tasks.Task DownloadVideoAsync(string url, StorageFile file)
        {
            // ダウンロード処理
            using var httpClient = new System.Net.Http.HttpClient();
            // HTTPリクエストをストリームで実行
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // ダウンロードされたストリームをローカルファイルに書き込む
            await using var inputStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = await file.OpenStreamForWriteAsync();
            await inputStream.CopyToAsync(fileStream); // ストリームを直接コピー
            await fileStream.FlushAsync(); // バッファをフラッシュ
        }
    }
}
