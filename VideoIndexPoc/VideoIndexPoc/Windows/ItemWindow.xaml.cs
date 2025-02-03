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

            // ���b�N�f�[�^��ǉ� (���ۂ�Azure API���g�p)
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

            // �f�[�^�o�C���h
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


                // �_�E�����[�h�����������ɒǉ�
                // �t�@�C���ۑ��_�C�A���O���쐬
                var savePicker = new FileSavePicker();

                // WinUI 3 �ł� Win32 ���œ��삷�邽�߁A�n���h����ݒ肷��K�v������܂��B
                //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_myMainWindow);
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
                        string downloadUrl = await _videoDownload.GetVideoDownloadUrl(account.Properties.Id, videoId, accountAccessToken, account.Location);
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
