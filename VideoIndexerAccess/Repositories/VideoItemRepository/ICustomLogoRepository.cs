using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface ICustomLogoRepository
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
    Task<LogoGroupContractModel> CreateLogoGroupAsync(LogoGroupRequestModel request);

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
    Task<LogoGroupContractModel> CreateLogoGroupAsync(string location, string accountId, LogoGroupRequestModel request, string? accessToken = null);

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

    /// <summary>
    /// API にロゴグループを削除するリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">削除するロゴグループのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    Task DeleteLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// 指定されたロゴIDに関連付けられたロゴを取得します。
    /// </summary>
    /// <remarks>
    /// このメソッドはアカウント情報を取得・検証し、アカウントのリージョンとID、アクセストークンを使用してロゴを取得します。
    /// <paramref name="logoId"/> が有効であること、必要なアカウント設定が正しく構成されていることを確認してください。
    /// </remarks>
    /// <param name="logoId">取得するロゴの一意な識別子。nullや空文字は不可。</param>
    /// <returns>ロゴの詳細を表す <see cref="LogoContractModel"/>。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合、または必要なアカウント情報がnullの場合にスローされます。</exception>
    /// 指定されたロゴIDに関連付けられたロゴを取得します。
    /// </summary>
    Task<LogoContractModel> GetLogoAsync(string logoId);

    /// <summary>
    /// ロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴレスポンスモデル</returns>
    Task<LogoContractModel> GetLogoAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// 指定されたロゴグループIDに関連付けられたロゴグループを取得します。
    /// </summary>
    /// <param name="logoGroupId">取得するロゴグループの一意な識別子。nullや空文字は不可。</param>
    /// <returns>ロゴグループの詳細を表す <see cref="LogoGroupContractModel"/>。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合、または必要なアカウント情報がnullの場合にスローされます。</exception>
    Task<LogoGroupContractModel> GetLogoGroupAsync(string logoGroupId);

    /// <summary>
    /// ロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴレスポンスモデル</returns>
    Task<LogoGroupContractModel> GetLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// <summary>
    /// 指定したロゴグループの関連ロゴを非同期で取得します。
    /// </summary>
    /// <remarks>
    /// このメソッドはアカウント情報を取得・検証し、アカウントのリージョンとID、アクセストークンを使用して指定したロゴグループの関連ロゴを取得します。
    /// </remarks>
    /// <param name="logoGroupId">関連ロゴを取得するロゴグループの一意な識別子。</param>
    /// <returns>指定したロゴグループに関連付けられたロゴ情報を含む <see cref="LogoGroupLinkedLogosModel"/>。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合にスローされます。</exception>
    Task<LogoGroupLinkedLogosModel> GetLogoGroupLinkedLogosAsync(string logoGroupId);

    /// <summary>
    /// ロゴグループリンクロゴを取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">取得するロゴグループのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴグループに関連するロゴグループリンクロゴ</returns>
    Task<LogoGroupLinkedLogosModel> GetLogoGroupLinkedLogosAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
    /// <returns>解析済みのロゴグループ情報のリスト</returns>
    /// </summary>
    Task<LogoGroupContractModel[]> GetLogoGroupsAsync();

    /// <summary>
    /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴグループ情報のリスト</returns>
    /// </summary>
    Task<LogoGroupContractModel[]> GetLogoGroupsAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// 指定したロゴIDに関連するロゴグループ情報を取得します。
    /// アカウント情報を取得し、APIを呼び出してロゴIDに関連するロゴグループ情報を取得します。
    /// </summary>
    /// <param name="logoId">関連グループを取得するロゴのID</param>
    /// <returns>ロゴIDに関連するロゴグループ情報の配列</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    Task<LogoGroupLinkedLogosModel[]> GetLogoLinkedGroupsAsync(string logoId);

    /// <summary>
    /// 指定したロゴIDに関連するロゴグループ情報を取得します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">関連グループを取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ロゴIDに関連するロゴグループ情報の配列</returns>
    Task<LogoGroupLinkedLogosModel[]> GetLogoLinkedGroupsAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// ロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴレスポンスモデル</returns>
    Task<LogoContractModel> GetLogosAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// ロゴ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="logoId">更新するロゴのID</param>
    /// <param name="updateRequest">更新するロゴ情報</param>
    /// <returns>更新後のロゴ情報</returns>
    Task<LogoContractModel> UpdateLogoAsync(string logoId, LogoUpdateRequestModel updateRequest);

    /// <summary>
    /// ロゴ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">更新するロゴのID</param>
    /// <param name="updateRequest">更新するロゴ情報</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>更新後のロゴ情報</returns>
    Task<LogoContractModel> UpdateLogoAsync(string location, string accountId, string logoId, LogoUpdateRequestModel updateRequest, string? accessToken = null);

    /// <summary>
    /// API にロゴグループ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="logoGroupId">更新するロゴグループのID</param>
    /// <param name="updateRequest">更新するロゴグループ情報</param>
    /// <returns>更新後のロゴグループ情報</returns>
    Task<LogoGroupContractModel> UpdateLogoGroupAsync(string logoGroupId, LogoGroupUpdateRequestModel updateRequest);

    /// <summary>
    /// API にロゴグループ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">更新するロゴグループのID</param>
    /// <param name="updateRequest">更新するロゴグループ情報</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>更新後のロゴグループ情報</returns>
    Task<LogoGroupContractModel> UpdateLogoGroupAsync(string location, string accountId, string logoGroupId, LogoGroupUpdateRequestModel updateRequest, string? accessToken = null);
}