using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IPromptContentRepository
{
    /// <summary>
    /// プロンプトコンテンツリポジトリクラス。
    /// Video Indexer API を使用してプロンプトコンテンツを取得するためのメソッドを提供します。
    /// </summary>
    /// <remarks>
    /// このクラスは、アカウント情報の取得、アクセストークンの管理、
    /// および API 呼び出しのロジックをカプセル化します。
    /// </remarks>
    Task<PromptContentContractModel?> GetPromptContentAsync(PromptContentRequestModel requestModel);

    /// <summary>
    /// 指定されたロケーション、アカウントID、リクエストモデル、およびアクセストークンを使用して
    /// プロンプトコンテンツを取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="requestModel">プロンプトコンテンツ取得リクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>取得したプロンプトコンテンツのレスポンスモデル、失敗時はnull</returns>
    /// <exception cref="ArgumentException">引数が無効な場合にスローされます</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされます</exception>
    /// <exception cref="Exception">その他のエラーが発生した場合にスローされます</exception>
    Task<PromptContentContractModel?> GetPromptContentAsync(string location, string accountId, PromptContentRequestModel requestModel, string? accessToken = null);

    /// <summary>
    /// 指定されたロケーション、アカウントID、ビデオID、モデル名、プロンプトスタイル、およびアクセストークンを使用して
    /// プロンプトコンテンツを作成します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="videoId">ビデオの一意の識別子</param>
    /// <param name="modelName">使用するLLMモデル名（省略可能）</param>
    /// <param name="promptStyle">プロンプトのスタイル（省略可能）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>プロンプトコンテンツの作成が成功した場合はtrue、それ以外はfalse</returns>
    /// <exception cref="ArgumentException">引数が無効な場合にスローされます</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされます</exception>
    /// <exception cref="Exception">その他のエラーが発生した場合にスローされます</exception>
    Task<bool> CreatePromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null);
}