using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class WidgetRepository
    {
        // ロガーインスタンス
        private readonly ILogger<WidgetRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API設定
        private readonly IWidgetsApiAccess _widgetsApiAccess;
        private const string ParamName = "widget";

        // マッパーインターフェース
        private readonly IVideoInsightsWidgetResponseMapper _insightsWidgetResponseMapper;

        public WidgetRepository(ILogger<WidgetRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IWidgetsApiAccess widgetsApiAccess, IVideoInsightsWidgetResponseMapper insightsWidgetResponseMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _widgetsApiAccess = widgetsApiAccess;
            _insightsWidgetResponseMapper = insightsWidgetResponseMapper;
        }


        public async Task<string?> GetVideoInsightsWidgetUrl(GetVideoInsightsWidgetResponseModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Insights Widget のURLを取得
            return await GetVideoInsightsWidgetUrl(location!, accountId!, requestModel, accessToken);
        }

        /// <summary>
        /// Insights Widget のURLを取得します。
        /// Video Indexer API から Insights Widget のリダイレクトURLを取得し返却します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="requestModel"></param>
        /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
        /// <returns>リダイレクトされた Insights Widget のURL。失敗時は null</returns>
        public async Task<string?> GetVideoInsightsWidgetUrl(string location, string accountId, GetVideoInsightsWidgetResponseModel requestModel, string? accessToken = null)
        {
            return await _widgetsApiAccess.GetVideoInsightsWidgetUrl(location, accountId, requestModel.VideoId, requestModel.WidgetType, requestModel.AllowEdit, accessToken);
        }


        /// <summary>
        /// Insights Widget の情報を取得します。
        /// アカウント情報の取得、検証、アクセストークンの取得を行い、Insights Widget API から情報を取得します。
        /// </summary>
        /// <param name="requestModel">ウィジェット情報取得用リクエストモデル</param>
        /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
        public async Task<VideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(GetVideoInsightsWidgetResponseModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // ウィジェット情報を取得
            return await GetVideoInsightsWidgetAsync(location!, accountId!, requestModel, accessToken);
        }

        /// <summary>
        /// Insights Widget の情報を取得します。
        /// </summary>
        /// <param name="requestModel">ウィジェット情報取得用リクエストモデル</param>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
        /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
        public async Task<VideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(string location, string accountId, GetVideoInsightsWidgetResponseModel requestModel, string? accessToken = null)
        {
            ApiVideoInsightsWidgetResponseModel? widgetResponse = await _widgetsApiAccess.GetVideoInsightsWidgetAsync(location, accountId, requestModel.VideoId, requestModel.WidgetType, requestModel.AllowEdit, accessToken);
            return widgetResponse is null ? null : _insightsWidgetResponseMapper.MapFrom(widgetResponse);
        }

        /// <summary>
        /// Video Player Widget のURLを取得します。
        /// アカウント情報の取得、検証、アクセストークンの取得を行い、Video Player Widget API からリダイレクトURLを取得します。
        /// </summary>
        /// <param name="videoId">対象のビデオID</param>
        /// <returns>リダイレクトされた Player Widget のURL。失敗時は null</returns>
        public async Task<string?> GetVideoPlayerWidgetUrl(string videoId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Player Widget のURLを取得
            return await GetVideoPlayerWidgetUrl(location!, accountId!, videoId, accessToken);
        }


        /// <summary>
        /// Video Player Widget のリダイレクトURLを取得します。
        /// Video Indexer API から Player Widget のリダイレクトURLを取得し返却します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象のビデオID</param>
        /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
        /// <returns>リダイレクトされた Player Widget のURL。失敗時は null</returns>
        public async Task<string?> GetVideoPlayerWidgetUrl(string location, string accountId, string videoId, string? accessToken = null)
        {
            return await _widgetsApiAccess.GetVideoPlayerWidgetUrl(location, accountId, videoId, accessToken);
        }


        /// <summary>
        /// Video Player Widget の情報を取得します。
        /// アカウント情報の取得、検証、アクセストークンの取得を行い、Video Player Widget API から情報を取得します。
        /// </summary>
        /// <param name="videoId">対象のビデオID</param>
        /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
        public async Task<ApiVideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(string videoId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Player Widget の情報を取得
            return await GetVideoPlayerWidgetAsync(location!, accountId!, videoId, accessToken);
        }

        /// <summary>
        /// Video Player Widget の情報を取得します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象のビデオID</param>
        /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
        /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
        public async Task<ApiVideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            return await _widgetsApiAccess.GetVideoPlayerWidgetAsync(location, accountId, videoId, accessToken);
        }
    }
}
