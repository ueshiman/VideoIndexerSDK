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

        //    // ���b�N�f�[�^��ǉ� (���ۂ�Azure API���g�p)
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

        //    // �f�[�^�o�C���h

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

                // �_�E�����[�h�����������ɒǉ�
                // �t�@�C���ۑ��_�C�A���O���쐬
                var savePicker = new FileSavePicker();

                // WinUI 3 �ł� Win32 ���œ��삷�邽�߁A�n���h����ݒ肷��K�v������܂��B
                //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MyMainWindow);
                // �t�@�C���^�C�v�ƃf�t�H���g�t�@�C������ݒ�
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
                    // �ۑ�������[�U�[�ɑI�΂���
                    StorageFile file = await savePicker.PickSaveFileAsync();

                    if (file != null)
                    {
                        // ��: �_�E�����[�h�����r�f�I��ۑ�
                        //string downloadUrlResponse = await _videoDownload.GetVideoDownloadUrl(account.Location, account.Properties.Id, videoId, accountAccessToken);
                        string downloadUrlResponse = await _videoDataRepository.GetVideoDownloadUrl(videoId);
                        string downloadUrl = downloadUrlResponse.Trim('"');
                        await DownloadVideoAsync(downloadUrl, file);

                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "����",
                            Content = "�r�f�I���ۑ�����܂����B",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };

                        await dialog.ShowAsync();
                    }
                    else
                    {
                        // ���[�U�[���L�����Z�������ꍇ
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "�L�����Z��",
                            Content = "�ۑ����L�����Z������܂����B",
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
                // �_�E�����[�h����
                using var httpClient = new System.Net.Http.HttpClient();
                // HTTP���N�G�X�g���X�g���[���Ŏ��s
                using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // �_�E�����[�h���ꂽ�X�g���[�������[�J���t�@�C���ɏ�������
                await using var inputStream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await inputStream.CopyToAsync(fileStream); // �X�g���[���𒼐ڃR�s�[
                await fileStream.FlushAsync(); // �o�b�t�@���t���b�V��
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        private async System.Threading.Tasks.Task DownloadVideoAsync(string url, StorageFile file)
        {
            // �_�E�����[�h����
            using var httpClient = new System.Net.Http.HttpClient();
            // HTTP���N�G�X�g���X�g���[���Ŏ��s
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // �_�E�����[�h���ꂽ�X�g���[�������[�J���t�@�C���ɏ�������
            await using var inputStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = await file.OpenStreamForWriteAsync();
            await inputStream.CopyToAsync(fileStream); // �X�g���[���𒼐ڃR�s�[
            await fileStream.FlushAsync(); // �o�b�t�@���t���b�V��
        }
    }
}
