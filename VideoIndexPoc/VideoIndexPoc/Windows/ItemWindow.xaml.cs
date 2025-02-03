using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient;
using VideoIndexPoc.VideoIndexerClient.ApiAccess;
using VideoIndexPoc.VideoIndexerClient.Auth;
using VideoIndexPoc.VideoIndexerClient.Model;
using VideoIndexPoc.VideoIndexerClient.Parser;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;
using System.Net.Http;
using ABI.Windows.Media.Capture;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexPoc.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemWindow : Window
    {
        private readonly IVideoItemAccess _videoItemAccess;
        private readonly IVideoItemParser _videoItemParser;
        private readonly IVideoIndexerClient _videoIndexerClient;
        private readonly IVideoDownload _videoDownload;
        public MainWindow? _myMainWindow { get; set; } = null;

        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        public ObservableCollection<VideoItem> VideoItems { get; set; } = new ObservableCollection<VideoItem>();

        public ItemWindow(IVideoItemAccess videoItemAccess, IVideoItemParser videoItemParser, IAccountTokenProviderDynamic accountTokenProvider, IVideoIndexerClient videoIndexerClient, IVideoDownload videoDownload)
        {
            _videoItemAccess = videoItemAccess;
            _videoItemParser = videoItemParser;
            _accountTokenProvider = accountTokenProvider;
            _videoIndexerClient = videoIndexerClient;
            _videoDownload = videoDownload;
            this.InitializeComponent();
        }


        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {

            // モックデータを追加 (実際はAzure APIを使用)
            VideoItems =
                [
                    new VideoItem()
                    {
                        summarizedInsights = new Summarizedinsights()
                        {
                            faces = new Face[]
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
                    }
                ];

            // データバインド
        }

        public async Task Load(string videoId)
        {
            var armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
            var accountAccessToken = await _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken);

            await _videoIndexerClient.AuthenticateAsync();

            var account = await _videoIndexerClient.GetAccountAsync(Constants.ViAccountName);

            var video = await _videoItemAccess.GetVideoItemAsync(account.Location, account.Properties.Id, videoId, accountAccessToken);
            VideoItems.Add(video);
            //VideoItems.Add(GetDummyItem());
        }

        private VideoItem GetDummyItem()
        {
            return new VideoItem()
            {
                summarizedInsights = new Summarizedinsights()
                {
                    faces = new Face[]
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
            if (sender is FrameworkElement element && element.DataContext is VideoItem videoItem)
            {
                var videoId = videoItem.id;
                var armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
                var accountAccessToken = await _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken);

                await _videoIndexerClient.AuthenticateAsync();

                var account = await _videoIndexerClient.GetAccountAsync(Constants.ViAccountName);

                var video = await _videoItemAccess.GetVideoItemAsync(account.Location, account.Properties.Id, videoId, accountAccessToken);


                // ダウンロード処理をここに追加
                // ファイル保存ダイアログを作成
                var savePicker = new FileSavePicker();

                // WinUI 3 では Win32 環境で動作するため、ハンドルを設定する必要があります。
                //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_myMainWindow);
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
                        string downloadUrl = await _videoDownload.GetVideoDownloadUrl(account.Properties.Id, videoId, accountAccessToken, account.Location);
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
