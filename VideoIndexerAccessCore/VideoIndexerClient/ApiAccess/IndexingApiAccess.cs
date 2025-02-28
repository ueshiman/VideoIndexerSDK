using Azure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class IndexingApiAccess
    {
        private readonly ILogger<IndexingApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly ISecureLogMessageBuilder _secureLogMessageBuilder;

        public IndexingApiAccess(ILogger<IndexingApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations, ISecureLogMessageBuilder secureLogMessageBuilder)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
            _secureLogMessageBuilder = secureLogMessageBuilder;
        }

        /// <summary>
        /// 特定の動画から顔情報を削除する。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">削除対象の動画 ID</param>
        /// <param name="faceId">削除対象の顔 ID</param>
        /// <param name="accessToken">(オプション) API アクセストークン</param>
        /// <returns>削除が成功した場合は true、それ以外は false</returns>
        public async Task<bool> DeleteVideoFaceAsync(string location, string accountId, string videoId, int faceId, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var url = $"{_apiResourceConfigurations}/{location}/Accounts/{accountId}/Videos/{videoId}/Index/Faces/{faceId}";

                // アクセストークンがある場合、クエリパラメータに追加
                url = _secureLogMessageBuilder.BuildRequestUri(url, accessToken, out var logUrl);

                // HTTP DELETE リクエストを作成
                using var request = new HttpRequestMessage(HttpMethod.Delete, logUrl);
                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                using var response = await httpClient.SendAsync(request);

                // ステータスコードの確認
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Face ID {FaceId} deleted successfully from video {VideoId}.", faceId, videoId);
                    return true;
                }

                // エラーハンドリング
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to delete face {FaceId} from video {VideoId}. Status Code: {StatusCode}, Error: {ErrorContent}", faceId, videoId, response.StatusCode, errorContent);

                return response.StatusCode switch
                {
                    // 各 HTTP ステータスコードに応じた例外処理
                    System.Net.HttpStatusCode.NotFound => throw new Exception($"Face ID {faceId} not found in video {videoId}."),
                    System.Net.HttpStatusCode.BadRequest => throw new Exception("Invalid request. Please check the parameters."),
                    System.Net.HttpStatusCode.Unauthorized => throw new Exception("Authentication failed. Please check the access token."),
                    System.Net.HttpStatusCode.TooManyRequests => throw new Exception("Too many requests. Please wait and try again."),
                    System.Net.HttpStatusCode.InternalServerError => throw new Exception("Internal server error. Please try again later."),
                    System.Net.HttpStatusCode.GatewayTimeout => throw new Exception("Gateway timeout. The server did not respond."),
                    _ => false
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: An error occurred while deleting Face ID {FaceId} from video {VideoId}.", faceId, videoId);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error: An error occurred while deleting Face ID {FaceId} from video {VideoId}.", faceId, videoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: An error occurred while deleting Face ID {FaceId} from video {VideoId}.", faceId, videoId);
                throw;
            }
        }

        /// <summary>
        /// 指定された動画の再インデックスを実行する。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">対象の動画 ID</param>
        /// <param name="accessToken">API アクセストークン</param>
        /// <param name="excludedAI">除外する AI のリスト（カンマ区切り）</param>
        /// <param name="isSearchable">検索可能にするか</param>
        /// <param name="indexingPreset">インデックスプリセット</param>
        /// <param name="streamingPreset">ストリーミングプリセット</param>
        /// <param name="callbackUrl">コールバック URL</param>
        /// <param name="sourceLanguage">ソース言語</param>
        /// <param name="sendSuccessEmail">成功時にメールを送信するか</param>
        /// <param name="linguisticModelId">言語モデル ID</param>
        /// <param name="personModelId">人物モデル ID</param>
        /// <param name="priority">処理優先度</param>
        /// <param name="brandsCategories">ブランドカテゴリ</param>
        /// <param name="customLanguages">カスタム言語</param>
        /// <param name="logoGroupId">ロゴグループ ID</param>
        /// <param name="punctuationMode">句読点モード</param>
        /// <returns>成功時は true、それ以外は false</returns>
        public async Task<bool> ReIndexVideoAsync(string location, string accountId, string videoId, string? accessToken = null, List<string>? excludedAI = null, bool? isSearchable = null, string? indexingPreset = null, string? streamingPreset = null, string? callbackUrl = null, string? sourceLanguage = null, bool? sendSuccessEmail = null, string? linguisticModelId = null, string? personModelId = null, string? priority = null, string? brandsCategories = null, string? customLanguages = null, string? logoGroupId = null, string? punctuationMode = null)
        {
            HttpResponseMessage? response;
            try
            {
                var baseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/ReIndex";
                var query = HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrEmpty(accessToken)) query["accessToken"] = accessToken;
                if (excludedAI != null && excludedAI.Count > 0)
                {
                    query["excludedAI"] = string.Join(",", excludedAI);
                }
                if (isSearchable.HasValue) query["isSearchable"] = isSearchable.Value.ToString().ToLower();
                if (!string.IsNullOrEmpty(indexingPreset)) query["indexingPreset"] = indexingPreset;
                if (!string.IsNullOrEmpty(streamingPreset)) query["streamingPreset"] = streamingPreset;
                if (!string.IsNullOrEmpty(callbackUrl)) query["callbackUrl"] = callbackUrl;
                if (!string.IsNullOrEmpty(sourceLanguage)) query["sourceLanguage"] = sourceLanguage;
                if (sendSuccessEmail.HasValue) query["sendSuccessEmail"] = sendSuccessEmail.Value.ToString().ToLower();
                if (!string.IsNullOrEmpty(linguisticModelId)) query["linguisticModelId"] = linguisticModelId;
                if (!string.IsNullOrEmpty(personModelId)) query["personModelId"] = personModelId;
                if (!string.IsNullOrEmpty(priority)) query["priority"] = priority;
                if (!string.IsNullOrEmpty(brandsCategories)) query["brandsCategories"] = brandsCategories;
                if (!string.IsNullOrEmpty(customLanguages)) query["customLanguages"] = customLanguages;
                if (!string.IsNullOrEmpty(logoGroupId)) query["logoGroupId"] = logoGroupId;
                if (!string.IsNullOrEmpty(punctuationMode)) query["punctuationMode"] = punctuationMode;

                var url = $"{baseUrl}?{query.ToString()}";
                if(query.Get("accessToken") is not null) query["accessToken"] = "***";
                var logUrl = $"{baseUrl}?{query.ToString()}";

                _logger.LogInformation("Sending request to {Url}", logUrl);

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: Failed to re-index video {VideoId}.", videoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: An error occurred while re-indexing video {VideoId}.", videoId);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Video {VideoId} re-indexing started successfully.", videoId);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to start re-indexing for video {VideoId}. Status Code: {StatusCode}, Error: {ErrorContent}", videoId, response.StatusCode, errorContent);
            return false;
        }


        /// <summary>
        /// 指定された動画の再インデックスを実行する。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">対象の動画 ID</param>
        /// <param name="accessToken">API アクセストークン</param>
        /// <param name="parameters">クエリパラメータの辞書</param>
        /// <returns>成功時は true、それ以外は false</returns>
        public async Task<bool> _ReIndexVideoAsync(string location, string accountId, string videoId, string? accessToken, Dictionary<string, string>? parameters = null)
        {
            HttpResponseMessage? response;
            try
            {
                var baseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/ReIndex";
                var query = HttpUtility.ParseQueryString(string.Empty);

                // アクセストークンをクエリに追加
                if(accessToken is not null)query["accessToken"] = accessToken;

                // 他のパラメータをクエリに追加
                if (parameters != null) foreach (var param in parameters) query[param.Key] = param.Value;

                var url = $"{baseUrl}?{query.ToString()}";

                using var request = new HttpRequestMessage(HttpMethod.Post, url);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: Failed to re-index video {VideoId}.", videoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: An error occurred while re-indexing video {VideoId}.", videoId);
                throw;
            }


            if (response is null) throw new HttpRequestException("The response was null.");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Video {VideoId} re-indexing started successfully.", videoId);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to start re-indexing for video {VideoId}. Status Code: {StatusCode}, Error: {ErrorContent}", videoId, response.StatusCode, errorContent);
            return false;
        }

        /// <summary>
        /// 指定された動画の顔情報を更新する。
        /// </summary>
        /// <param name="location">Azure のリージョン（例: eastus, westeurope）</param>
        /// <param name="accountId">Video Indexer のアカウント ID（GUID）</param>
        /// <param name="videoId">更新対象の動画 ID</param>
        /// <param name="faceId">更新対象の顔 ID（整数）</param>
        /// <param name="newName">新しい名前（オプション）</param>
        /// <param name="personId">更新する人物 ID（GUID, オプション）</param>
        /// <param name="createNewPerson">新しい人物を作成するかどうか（オプション）</param>
        /// <param name="accessToken">API アクセストークン（オプション, 有効期限 1 時間）</param>
        /// <returns>更新成功時は true、それ以外は false</returns>
        public async Task<bool> UpdateVideoFaceAsync(string location, string accountId, string videoId, int faceId, string? newName = null, string? personId = null, bool? createNewPerson = null, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                // API エンドポイントのベース URL を構築
                var baseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Index/Faces/{faceId}";
                var query = HttpUtility.ParseQueryString(string.Empty);

                // クエリパラメータの設定（オプション）
                if (!string.IsNullOrEmpty(newName)) query["newName"] = newName;
                if (!string.IsNullOrEmpty(personId)) query["personId"] = personId;
                if (createNewPerson.HasValue) query["createNewPerson"] = createNewPerson.Value.ToString().ToLower();

                var baseQueryString = query.ToString();
                var accessTokenKey = "accessToken";

                // API リクエスト用の完全なクエリ文字列を構築
                var finalQueryString = baseQueryString;
                if (!string.IsNullOrEmpty(accessToken)) finalQueryString += string.IsNullOrEmpty(finalQueryString) ? $"{accessTokenKey}={accessToken}" : $"&{accessTokenKey}={accessToken}";

                // ログ用に accessToken をマスクしたクエリ文字列を作成
                var maskedQueryString = baseQueryString;
                if (!string.IsNullOrEmpty(accessToken)) maskedQueryString += string.IsNullOrEmpty(maskedQueryString) ? $"{accessTokenKey}=****" : $"&{accessTokenKey}=****";

                var url = string.IsNullOrEmpty(finalQueryString) ? baseUrl : $"{baseUrl}?{finalQueryString}";
                var maskedUrl = string.IsNullOrEmpty(maskedQueryString) ? baseUrl : $"{baseUrl}?{maskedQueryString}";

                _logger.LogInformation("Request URL: {MaskedUrl}", maskedUrl);

                // HTTP PUT リクエストを送信
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.SendAsync(request);

                if (response is null) throw new HttpRequestException("The response was null.");

                // 成功時の処理
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Face {FaceId} updated successfully in video {VideoId}.", faceId, videoId);
                    return true;
                }

                // 失敗時のエラーログ出力
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update face {FaceId} in video {VideoId}. Status Code: {StatusCode}, Error: {ErrorContent}", faceId, videoId, response.StatusCode, errorContent);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: Failed to update face {FaceId} in video {VideoId}.", faceId, videoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: An error occurred while updating face {FaceId} in video {VideoId}.", faceId, videoId);
                throw;
            }
        }

        /// <summary>
        /// 指定されたパッチ操作を使用してビデオのインデックスを更新します。
        /// </summary>
        /// <param name="location">リクエストを送信する Azure リージョン。</param>
        /// <param name="accountId">Video Indexer サービスに関連付けられたアカウント ID。</param>
        /// <param name="videoId">インデックスを更新する対象のビデオ ID。</param>
        /// <param name="patchOperations">適用する JSON Patch ドキュメント。</param>
        /// <param name="language">(オプション) インデックスを取得する言語。</param>
        /// <param name="accessToken">(オプション) 認証に必要なアクセストークン。</param>
        /// <returns>更新が成功した場合は true、失敗した場合は false を返します。</returns>
        public async Task<bool> UpdateVideoIndexAsync(string location, string accountId, string videoId, List<object> patchOperations, string? language = null, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                // APIのベースURLを構築
                var baseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Index";
                var query = HttpUtility.ParseQueryString(string.Empty);

                // 言語パラメータの設定
                if (!string.IsNullOrEmpty(language))
                {
                    query["language"] = language;
                }

                // クエリ文字列の組み立て
                var baseQueryString = query.ToString();
                var accessTokenKey = "accessToken";

                // マスク前のクエリ文字列
                var finalQueryString = baseQueryString;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    finalQueryString += string.IsNullOrEmpty(finalQueryString)
                        ? $"{accessTokenKey}={accessToken}"
                        : $"&{accessTokenKey}={accessToken}";
                }

                // マスク後のクエリ文字列
                var maskedQueryString = baseQueryString;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    maskedQueryString += string.IsNullOrEmpty(maskedQueryString)
                        ? $"{accessTokenKey}=***"
                        : $"&{accessTokenKey}=***";
                }

                // URLの組み立て
                var url = string.IsNullOrEmpty(finalQueryString) ? baseUrl : $"{baseUrl}?{finalQueryString}";
                var maskedUrl = string.IsNullOrEmpty(maskedQueryString) ? baseUrl : $"{baseUrl}?{maskedQueryString}";

                _logger.LogInformation("Request URL: {MaskedUrl}", maskedUrl);

                // HTTPリクエストの準備
                var jsonContent = JsonSerializer.Serialize(patchOperations);
                using var request = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.SendAsync(request);
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Video index updated successfully for video {VideoId}.", videoId);
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update video index for video {VideoId}. Status Code: {StatusCode}, Error: {ErrorContent}", videoId, response.StatusCode, errorContent);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: Failed to update video index for video {VideoId}.", videoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: An error occurred while updating video index for video {VideoId}.", videoId);
                throw;
            }
        }

    }
}
