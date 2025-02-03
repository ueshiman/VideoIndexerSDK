namespace VideoIndexerAccessCore.VideoIndexerClient.Configuration
{
    public class ApiResourceConfigurations : IApiResourceConfigurations
    {
        public const string ApiVersionDefault = "2022-08-01";
        public const string AzureResourceManagerDefault = "https://management.azure.com";
        public const string ApiEndpointDefault = "https://api.videoindexer.ai";
        public const string DefaultHttpClientNameDefault = "AccountAccess-client";

        public const string SubscriptionIdKey = "SUBSCRIPTION_ID";
        public const string ResourceGroupKey = "VI_RESOURCE_GROUP";
        public const string ViAccountNameKey = "VI_ACCOUNT_NAME";
        public const string ApiVersionKey = "API_VERSION";
        public const string AzureResourceKey = "AZURE_RESOURCE";
        public const string ApiEndpointKey = "API_ENDPOINT";
        public const string DefaultHttpClientNameKey = "DEFAULT_HTTP_CLIENT_NAME";

        /// <summary>
        /// Video Indexer API Version
        /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
        /// </summary>
        public string ApiVersion => _apiVersion;
        /// <summary>
        /// Video Indexer リソース名
        /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
        /// </summary>
        public string AzureResource => _azureResource;
        /// <summary>
        /// Video Indexer APIエンドポイント
        /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
        /// </summary>
        public string ApiEndpoint => _apiEndpoint;
        /// <summary>
        /// Video IndexerのサブスクリプションのGUID
        /// コンストラクターで設定された値がない場合は環境変数から取得
        /// </summary>
        public string? SubscriptionId => _subscriptionId;
        /// <summary>
        /// Video Indexerのリソース名
        /// コンストラクターで設定された値がない場合は環境変数から取得
        /// </summary>
        public string? ResourceGroup => _resourceGroup;
        /// <summary>
        /// アカウント名
        /// コンストラクターで設定された値がない場合は環境変数から取得
        /// </summary>
        public string? ViAccountName => _viAccountName;

        /// <summary>
        /// HttpClient名
        /// コンストラクターで設定された値がない場合は環境変数から取得
        /// </summary>
        public string DefaultHttpClientName => _defaultHttpClientName;

        public readonly string _apiVersion;
        public readonly string _azureResource;
        public readonly string _apiEndpoint;
        public readonly string? _subscriptionId;
        public readonly string? _resourceGroup;
        public readonly string? _viAccountName;
        public readonly string _defaultHttpClientName;

        //public readonly string ApiEndpoint = Environment.GetEnvironmentVariable("API_ENDPOINT") ?? ApiEndpointDefault;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="apiVersion">Video Indexer API Version</param>
        /// <param name="azureResource">Video Indexer リソース名</param>
        /// <param name="apiEndpoint">エンドポイント</param>
        /// <param name="subscriptionId">Video IndexerのサブスクリプションのGUID</param>
        /// <param name="resourceGroup">Video Indexerのリソース名</param>
        /// <param name="viAccountName">アカウント名</param>
        public ApiResourceConfigurations(string? defaultHttpClientName = null, string? apiVersion = null, string? azureResource = null, string? apiEndpoint = null, string? subscriptionId = null, string? resourceGroup = null, string? viAccountName = null)
        {
            _apiVersion = apiVersion ?? Environment.GetEnvironmentVariable(ApiVersionKey) ?? ApiVersionDefault;
            _azureResource = azureResource ?? Environment.GetEnvironmentVariable(AzureResourceKey) ?? AzureResourceManagerDefault;
            _apiEndpoint = apiEndpoint ?? Environment.GetEnvironmentVariable(ApiEndpointKey) ?? ApiEndpointDefault;
            _subscriptionId = subscriptionId ?? Environment.GetEnvironmentVariable(SubscriptionIdKey);
            _resourceGroup = resourceGroup ?? Environment.GetEnvironmentVariable(ResourceGroupKey);
            _viAccountName = viAccountName ?? Environment.GetEnvironmentVariable(ViAccountNameKey);
            _defaultHttpClientName = defaultHttpClientName ?? Environment.GetEnvironmentVariable(DefaultHttpClientNameKey) ?? DefaultHttpClientNameDefault;
        }

        /// <summary>
        /// バリデーション
        /// 最低限必要な設定がされているか
        /// </summary>
        /// <returns></returns>
        public bool Valid() => !string.IsNullOrWhiteSpace(_subscriptionId) && !string.IsNullOrWhiteSpace(_resourceGroup) && !string.IsNullOrWhiteSpace(_viAccountName);
    }
}
