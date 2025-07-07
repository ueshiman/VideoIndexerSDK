using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface ICreateLogoRepository
{
    /// <summary>
    /// カスタムロゴを作成する非同期メソッド。
    /// アカウント情報を取得し、APIを呼び出してロゴを作成します。
    /// </summary>
    /// <param name="request">ロゴ作成リクエストモデル</param>
    /// <returns>作成されたロゴのレスポンスモデル</returns>
    Task<LogoContractModel> CreateCustomLogoAsync(LogoRequestModel request);

    /// <summary>
    /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでカスタムロゴを作成します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ロゴ作成リクエストモデル</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>作成されたロゴのレスポンスモデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LogoContractModel> CreateCustomLogoAsync(string location, string accountId, LogoRequestModel request, string? accessToken = null);

    /// <summary>
    /// カスタムロゴグループを作成する非同期メソッド。
    /// アカウント情報を取得し、APIを呼び出してロゴグループを作成します。
    /// </summary>
    /// <param name="request">ロゴグループ作成リクエストモデル</param>
    /// <returns>作成されたロゴグループのレスポンスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    Task<ApiLogoLogoGroupContractModel> CreateLogoGroupAsync(LogoGroupRequestModel request);

    /// <summary>
    /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでロゴグループを作成します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ロゴグループ作成リクエストモデル</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>作成されたロゴグループのレスポンスモデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<ApiLogoLogoGroupContractModel> CreateLogoGroupAsync(string location, string accountId, LogoGroupRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定したロゴIDのロゴを削除します。
    /// アカウント情報を取得し、APIを呼び出してロゴを削除します。
    /// </summary>
    /// <param name="logoId">削除するロゴのID</param>
    /// <returns>非同期タスク</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task DeleteLogoAsync(string logoId);

    /// <summary>
    /// 指定したロケーション・アカウントID・ロゴID・アクセストークンでロゴを削除します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">削除するロゴのID</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>非同期タスク</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task DeleteLogoAsync(string location, string accountId, string logoId, string? accessToken = null);
}