namespace VideoIndexPoc2.VideoIndexerClient.Configuration;

public interface IApiResourceConfigurations
{
    /// <summary>
    /// Video Indexer API Version
    /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
    /// </summary>
    string ApiVersion { get; }

    /// <summary>
    /// Video Indexer リソース名
    /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
    /// </summary>
    string AzureResource { get; }

    /// <summary>
    /// Video Indexer APIエンドポイント
    /// コンストラクターで設定された値がない場合は環境変数から取得、それもない場合はデフォルト値を使用
    /// </summary>
    string ApiEndpoint { get; }

    /// <summary>
    /// Video IndexerのサブスクリプションのGUID
    /// コンストラクターで設定された値がない場合は環境変数から取得
    /// </summary>
    string? SubscriptionId { get; }

    /// <summary>
    /// Video Indexerのリソース名
    /// コンストラクターで設定された値がない場合は環境変数から取得
    /// </summary>
    string? ResourceGroup { get; }

    /// <summary>
    /// アカウント名
    /// コンストラクターで設定された値がない場合は環境変数から取得
    /// </summary>
    string? ViAccountName { get; }

    /// <summary>
    /// HttpClient名
    /// コンストラクターで設定された値がない場合は環境変数から取得
    /// </summary>
    string? DefaultHttpClientName { get; }

    /// <summary>
    /// バリデーション
    /// 最低限必要な設定がされているか
    /// </summary>
    /// <returns></returns>
    bool Valid();
}