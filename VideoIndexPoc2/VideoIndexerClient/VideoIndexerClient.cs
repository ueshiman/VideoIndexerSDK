using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexPoc2.VideoIndexerClient.Authorization;
using VideoIndexPoc2.VideoIndexerClient.Configuration;
using VideoIndexPoc2.VideoIndexerClient.HttpAccess;
using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient
{
    public class VideoIndexerClient : IVideoIndexerClient
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private string _armAccessToken;
        private string _accountAccessToken;
        private Account _account;

        private readonly TimeSpan _pollingInteval = TimeSpan.FromSeconds(10);

        public VideoIndexerClient(IAccountTokenProviderDynamic accountTokenProvider, IHttpClientFactory httpClientFactory, IApiResourceConfigurations apiResourceConfigurations)
        {
            _accountTokenProvider = accountTokenProvider;
            _httpClientFactory = httpClientFactory;
            _apiResourceConfigurations = apiResourceConfigurations;
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
            _httpClient = _httpClientFactory.CreateClient("my-client");
            //_httpClient = HttpClientHelper.CreateHttpClient();
        }

        public async Task AuthenticateAsync()
        {
            try
            {
                _armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
                _accountAccessToken = await _accountTokenProvider.GetAccountAccessTokenAsync(_armAccessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void Authenticate()
        {
            try
            {
                _armAccessToken = _accountTokenProvider.GetArmAccessToken();
                _accountAccessToken = _accountTokenProvider.GetAccountAccessTokenAsync(_armAccessToken).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get Information about the Account
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountAsync(string accountName)
        {
            if (_account != null)
            {
                return _account;
            }
            Console.WriteLine($"Getting account {accountName}.");
            try
            {
                // Set request uri
                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{accountName}?api-version={_apiResourceConfigurations.ApiVersion}";
                HttpClient client = _httpClientFactory.CreateClient("my-client");
                //var client = HttpClientHelper.CreateHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _armAccessToken);

                var result = await client.GetAsync(requestUri);

                result.VerifyStatus(System.Net.HttpStatusCode.OK);
                var jsonResponseBody = await result.Content.ReadAsStringAsync();
                var account = JsonSerializer.Deserialize<Account>(jsonResponseBody);
                VerifyValidAccount(account, accountName);
                Console.WriteLine($"[Account Details] Id:{account.Properties.Id}, Location: {account.Location}");
                _account = account;
                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }



        /// <summary>
        /// Uploads a video and starts the video index. Calls the uploadVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Upload-Video)
        /// </summary>
        /// <param name="videoUrl"> Link To Publicy Accessed Video URL</param>
        /// <param name="videoName"> The Asset name to be used </param>
        /// <param name="exludedAIs"> The ExcludeAI list to run </param>
        /// <param name="waitForIndex"> should this method wait for index operation to complete </param>
        /// <exception cref="Exception"></exception>
        /// <returns> Video Id of the video being indexed, otherwise throws excpetion</returns>
        public async Task<string> UploadUrlAsync(string videoUrl , string videoName, string exludedAIs = null, bool waitForIndex = false )
        {
            if (_account == null)
            {
                throw new Exception("Call Get Account Details First");
            }

            Console.WriteLine($"Video for account {_account.Properties.Id} is starting to upload.");
            
            try
            {
                //Build Query Parameter Dictionary
                var queryDictionary = new Dictionary<string, string>
                {
                    { "name", videoName },
                    { "description", "video_description" },
                    { "privacy", "private" },
                    { "accessToken" , _accountAccessToken },
                    { "videoUrl" , videoUrl },
                    { "language", "multi" }
                };

                if (!Uri.IsWellFormedUriString(videoUrl, UriKind.Absolute))
                {
                    throw new ArgumentException("VideoUrl or LocalVidePath are invalid");
                }
                
                var queryParams = queryDictionary.CreateQueryString();
                if (!string.IsNullOrEmpty(exludedAIs))
                    queryParams += AddExcludedAIs(exludedAIs);

                // Send POST request
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos?{queryParams}";
                var uploadRequestResult = await _httpClient.PostAsync(url, null);
                uploadRequestResult.VerifyStatus(System.Net.HttpStatusCode.OK);
                var uploadResult = await uploadRequestResult.Content.ReadAsStringAsync();

                // Get the video ID from the upload result
                var videoId = JsonSerializer.Deserialize<Video>(uploadResult).Id;
                Console.WriteLine($"Video ID {videoId} was uploaded successfully");
                
                if (waitForIndex)
                {
                    Console.WriteLine("Waiting for Index Operation to Complete");
                    await WaitForIndexAsync(videoId);
                }
                return videoId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Uploads a video and starts the video index. Calls the uploadVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Upload-Video)
        /// </summary>
        /// <param name="filePath"> Link To Publicy Accessed Video URL</param>
        /// <param name="videoName"> The Asset name to be used </param>
        /// <param name="exludedAIs"> The ExcludeAI list to run </param>
        /// <param name="waitForIndex"> should this method wait for index operation to complete </param>
        /// <exception cref="Exception"></exception>
        /// <returns> Video Id of the video being indexed, otherwise throws excpetion</returns>
        public async Task<string> UploadFileAsync(string filePath, string videoName, string exludedAIs = null, bool waitForIndex = false)
        {
            if (_account == null)
            {
                throw new Exception("Call Get Account Details First");
            }

            Console.WriteLine($"Video for account {_account.Properties.Id} is starting to upload.");
            //Build Query Parameter Dictionary
            var queryDictionary = new Dictionary<string, string>
            {
                { "name", videoName },
                { "description", "video_description" },
                { "privacy", "private" },
                { "accessToken" , _accountAccessToken },
                //{ "videoUrl" , videoUrl },
                { "language", "multi" }
            };
            var queryParams = queryDictionary.CreateQueryString();
            if (!string.IsNullOrEmpty(exludedAIs))
                queryParams += AddExcludedAIs(exludedAIs);
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos?{queryParams}";
            //string url = $"{ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos?name={videoName}&privacy=Private";

            //using (HttpClient client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accountAccessToken}");
            //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            //    using (var form = new MultipartFormDataContent())
            //    {
            //        // ビデオファイルをストリームとして読み込む
            //        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //        {
            //            var streamContent = new StreamContent(fileStream);
            //            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("video/mp4");
            //            form.Add(streamContent, "file", Path.GetFileName(filePath));

            //            // POSTリクエストでビデオをアップロード
            //            HttpResponseMessage response = await client.PostAsync(url, form);
            //            response.EnsureSuccessStatusCode();

            //            // レスポンスの確認
            //            string responseBody = await response.Content.ReadAsStringAsync();
            //            Console.WriteLine("Upload response: " + responseBody);

            //            var videoId = JsonSerializer.Deserialize<Video>(responseBody).Id;

            //            return videoId;
            //        }
            //    }
            //}


            try
            {
                ////Build Query Parameter Dictionary
                //var queryDictionary = new Dictionary<string, string>
                //{
                //    { "name", videoName },
                //    { "description", "video_description" },
                //    { "privacy", "private" },
                //    { "accessToken" , _accountAccessToken },
                //    { "language", "multi" }
                //};

                //if (!Uri.IsWellFormedUriString(videPath, UriKind.Absolute))
                //{
                //    throw new ArgumentException("VideoUrl or LocalVidePath are invalid");
                //}

                //var queryParams = queryDictionary.CreateQueryString();
                //if (!string.IsNullOrEmpty(exludedAIs))
                //    queryParams += AddExcludedAIs(exludedAIs);

                //// Send POST request
                //var url = $"{ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos?{queryParams}";

                using var form = new MultipartFormDataContent();
                // ビデオファイルをストリームとして読み込む
                await using var fileStream = new FileStream(filePath, FileMode.Open, System.IO.FileAccess.ReadWrite );
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("video/mp4");
                form.Add(streamContent, "file", Path.GetFileName(filePath));

                // POSTリクエストでビデオをアップロード
                var uploadRequestResult = await _httpClient.PostAsync(url, form);

                //uploadRequestResult.EnsureSuccessStatusCode();
                //// レスポンスの確認
                //string responseBody = await uploadRequestResult.Content.ReadAsStringAsync();
                //Console.WriteLine("Upload response: " + responseBody);

                uploadRequestResult.VerifyStatus(System.Net.HttpStatusCode.OK);
                var uploadResult = await uploadRequestResult.Content.ReadAsStringAsync();

                // Get the video ID from the upload result
                var videoId = JsonSerializer.Deserialize<Video>(uploadResult).Id;
                Console.WriteLine($"Video ID {videoId} was uploaded successfully");

                if (waitForIndex)
                {
                    Console.WriteLine("Waiting for Index Operation to Complete");
                    await WaitForIndexAsync(videoId);
                }
                return videoId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Calls getVideoIndex API in 10 second intervals until the indexing state is 'processed'(https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Index)
        /// </summary>
        /// <param name="videoId"> The video id </param>
        /// <exception cref="Exception"></exception>
        /// <returns> Prints video index when the index is complete, otherwise throws exception </returns>
        public async Task WaitForIndexAsync(string videoId)
        {
            Console.WriteLine($"Waiting for video {videoId} to finish indexing.");
            while (true)
            {
                var queryParams = new Dictionary<string, string>()
                {
                    //{"language", "English"},
                    {"language", "ja-JP"},
                    { "accessToken" , _accountAccessToken },
                }.CreateQueryString();

                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos/{videoId}/Index?{queryParams}";
                var videoGetIndexRequestResult = await _httpClient.GetAsync(requestUrl);
                videoGetIndexRequestResult.VerifyStatus(System.Net.HttpStatusCode.OK);
                var videoGetIndexResult = await videoGetIndexRequestResult.Content.ReadAsStringAsync();
                var processingState = JsonSerializer.Deserialize<Video>(videoGetIndexResult).State;

                // If job is finished
                if (processingState == ProcessingState.Processed.ToString())
                {
                    Console.WriteLine($"The video index has completed. Here is the full JSON of the index for video ID {videoId}: \n{videoGetIndexResult}");
                    return;
                }
                else if (processingState == ProcessingState.Failed.ToString())
                {
                    Console.WriteLine($"The video index failed for video ID {videoId}.");
                    throw new Exception(videoGetIndexResult);
                }

                // Job hasn't finished
                Console.WriteLine($"The video index state is {processingState}");
                await Task.Delay(_pollingInteval);
            }
        }

        /// <summary>
        /// Searches for the video in the account. Calls the searchVideo API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos)
        /// </summary>
        /// <param name="videoId"> The video id </param>
        /// <returns> Prints the video metadata, otherwise throws excpetion</returns>
        public async Task GetVideoAsync(string videoId)
        {
            Console.WriteLine($"Searching videos in account {_account.Properties.Id} for video ID {videoId}.");
            var queryParams = new Dictionary<string, string>()
            {
                {"id", videoId},
                { "accessToken" , _accountAccessToken },
                //{"language","ar-AE"}
            }.CreateQueryString();

            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos/Search?{queryParams}";
                var searchRequestResult = await _httpClient.GetAsync(requestUrl);
                searchRequestResult.VerifyStatus(System.Net.HttpStatusCode.OK);
                var searchResult = await searchRequestResult.Content.ReadAsStringAsync();
                Console.WriteLine($"Here are the search results: \n{searchResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<string> FileUploadAsync(string videoName,  string mediaPath, string exludedAIs = null)
        {
            if (!File.Exists(mediaPath))
                throw new Exception($"Could not find file at path {mediaPath}");

            var queryParams = new Dictionary<string, string>
            {
                { "name", videoName },
                { "description", "video_description" },
                { "privacy", "private" },
                { "accessToken" , _accountAccessToken },
                { "partition", "partition" }
            }.CreateQueryString();
            
            if (!string.IsNullOrEmpty(exludedAIs))
                queryParams += AddExcludedAIs(exludedAIs);
            
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos?{queryParams}";
            // Create multipart form data content
            using var content = new MultipartFormDataContent();
            // Add file content
            await using var fileStream = new FileStream(mediaPath, FileMode.Open, System.IO.FileAccess.ReadWrite);
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "fileName", Path.GetFileName(mediaPath));
            Console.WriteLine("Uploading a local file using multipart/form-data post request..");
            // Send POST request
            var response = await _httpClient.PostAsync(url, content);
            Console.WriteLine(response.Headers.ToString());
            // Process response
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            return response.ToString();
        }

        /// <summary>
        /// Calls the getVideoInsightsWidget API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Insights-Widget)
        /// </summary>
        /// <param name="videoId"> The video id </param>
        /// <returns> Prints the VideoInsightsWidget URL, otherwise throws exception</returns>
        public async Task GetInsightsWidgetUrlAsync(string videoId)
        {
            Console.WriteLine($"Getting the insights widget URL for video {videoId}");
            var queryParams = new Dictionary<string, string>()
            {
                {"widgetType", "Keywords"},
                { "accessToken" , _accountAccessToken },
                {"allowEdit", "true"},
            }.CreateQueryString();
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos/{videoId}/InsightsWidget?{queryParams}";
                var insightsWidgetRequestResult = await _httpClient.GetAsync(requestUrl);
                insightsWidgetRequestResult.VerifyStatus(System.Net.HttpStatusCode.MovedPermanently);
                var insightsWidgetLink = insightsWidgetRequestResult.Headers.Location;
                Console.WriteLine($"Got the insights widget URL: \n{insightsWidgetLink}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Calls the getVideoPlayerWidget API (https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Player-Widget)
        /// </summary>
        /// <param name="videoId"> The video id </param>
        /// <returns> Prints the VideoPlayerWidget URL, otherwise throws exception</returns>
        public async Task GetPlayerWidgetUrlAsync( string videoId)
        {
            Console.WriteLine($"Getting the player widget URL for video {videoId}");
            
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{_account.Location}/Accounts/{_account.Properties.Id}/Videos/{videoId}/PlayerWidget";
                var playerWidgetRequestResult = await _httpClient.GetAsync(requestUrl);

                var playerWidgetLink = playerWidgetRequestResult.Headers.Location;
                playerWidgetRequestResult.VerifyStatus(System.Net.HttpStatusCode.MovedPermanently);
                Console.WriteLine($"Got the player widget URL: \n{playerWidgetLink}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        private string AddExcludedAIs(string ExcludedAI)
        {
            if (string.IsNullOrEmpty(ExcludedAI))
            {
                return "";
            }
            var list = ExcludedAI.Split(',');
            return list.Aggregate("", (current, item) => current + ("&ExcludedAI=" + item));
        }

        private static void VerifyValidAccount(Account account,string accountName)
        {
            if (string.IsNullOrWhiteSpace(account.Location) || account.Properties == null || string.IsNullOrWhiteSpace(account.Properties.Id))
            {
                Console.WriteLine($"{nameof(accountName)} {accountName} not found. Check {nameof(_apiResourceConfigurations.SubscriptionId)}, {nameof(_apiResourceConfigurations.ResourceGroup)}, {nameof(accountName)} ar valid.");
                throw new Exception($"Account {accountName} not found.");
            }
        }

        public Account GetAccount(string accountName)
        {
            throw new NotImplementedException();
        }
    }

    //public static class Constants
    //{
    //    public const string ApiVersion = "2022-08-01";
    //    public const string AzureResourceManager = "https://management.azure.com";
    //    public static readonly string SubscriptionId = Environment.GetEnvironmentVariable("SUBSCIPTION_ID");
    //    public static readonly string ResourceGroup = Environment.GetEnvironmentVariable("VI_RESOURCE_GROUP");
    //    public static readonly string ViAccountName = Environment.GetEnvironmentVariable("VI_ACCOUNT_NAME");
    //    public static readonly string ApiEndpoint = Environment.GetEnvironmentVariable("API_ENDPOINT") ?? "https://api.videoindexer.ai";

    //    public static bool Valid() => !string.IsNullOrWhiteSpace(SubscriptionId) &&
    //                                  !string.IsNullOrWhiteSpace(ResourceGroup) &&
    //                                  !string.IsNullOrWhiteSpace(ViAccountName);
    //}
}
