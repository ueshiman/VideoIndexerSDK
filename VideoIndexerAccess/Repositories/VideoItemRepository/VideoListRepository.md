
# VideoListRepository クラス

`VideoListRepository` クラスは、ビデオリストに関するデータ操作を行うリポジトリクラスです。このクラスは、ビデオインデクサーAPIを使用してビデオリストを取得、検索、およびパースする機能を提供します。

## コンストラクタ

public VideoListRepository( ILogger? logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IVideoListApiAccess videoListApiAccess, IVideoItemDataMapper videoItemDataMapper, IVideoListDataModelMapper videoListDataModelMapper, IAccountRepository accountRepository)


### パラメータ

- `logger`: ロガーインスタンス。
- `authenticationTokenizer`: 認証トークンプロバイダー。
- `accountAccess`: アカウントアクセスプロバイダー。
- `videoListApiAccess`: ビデオリストAPIアクセスプロバイダー。
- `videoItemDataMapper`: ビデオアイテムデータマッパー。
- `videoListDataModelMapper`: ビデオリストデータモデルマッパー。
- `accountRepository`: アカウントリポジトリ。

## メソッド

### `GetVideoListJsonAsync`

public async Task GetVideoListJsonAsync(string? location, string? accountId)


AzureリージョンとアカウントIDを指定して、ビデオリストのJSONレスポンスを非同期で取得します。

### パラメータ

- `location`: Azure リージョン。
- `accountId`: アカウント ID。

### 戻り値

- `Task<string>`: ビデオリストのJSONレスポンス。

### `ListVideosAsync`

public async Task<IEnumerable> ListVideosAsync( DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null)


指定されたフィルタ条件に基づいて、ビデオリストを非同期で取得し、パースして返します。

### パラメータ

- `createdAfter`: 指定された日付以降に作成されたビデオをフィルタリング。
- `createdBefore`: 指定された日付以前に作成されたビデオをフィルタリング。
- `pageSize`: ページサイズ。
- `skip`: スキップするレコード数。
- `partitions`: パーティションでビデオをフィルタリング。

### 戻り値

- `Task<IEnumerable<VideoListDataModel>>`: ビデオリスト。

### `SearchVideosAsync`

public async Task SearchVideosAsync( string? sourceLanguage = null, bool? hasSourceVideoFile = null, string? sourceVideoId = null, string[]? state = null, string[]? privacy = null, string[]? id = null, string[]? partition = null, string[]? externalId = null, string[]? owner = null, string[]? face = null, string[]? animatedCharacter = null, string[]? query = null, string[]? textScope = null, string[]? language = null, DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null)


指定された条件でビデオを検索し、検索結果のビデオリストのJSONレスポンスを非同期で取得します。

### パラメータ

- `sourceLanguage`: ソース言語。
- `hasSourceVideoFile`: ソースビデオファイルの有無。
- `sourceVideoId`: ソースビデオID。
- `state`: ビデオの状態。
- `privacy`: プライバシーレベル。
- `id`: ビデオID。
- `partition`: パーティション。
- `externalId`: 外部ID。
- `owner`: 所有者。
- `face`: 顔。
- `animatedCharacter`: アニメキャラクター。
- `query`: クエリ。
- `textScope`: テキストスコープ。
- `language`: 言語。
- `createdAfter`: 作成日（以降）。
- `createdBefore`: 作成日（以前）。
- `pageSize`: ページサイズ。
- `skip`: スキップ数。

### 戻り値

- `Task<string>`: 検索結果のビデオリストのJSONレスポンス。

## 例外処理

各メソッドは、例外が発生した場合にロガーを使用してエラーメッセージを記録し、例外を再スローします。

catch (Exception ex) { _logger?.LogError(ex, "An error occurred while listing videos."); throw; }


## 依存関係

このクラスは以下のインターフェースおよびクラスに依存しています。

- `ILogger<VideoListRepository>`
- `IAuthenticationTokenizer`
- `IAccounApitAccess`
- `IVideoListApiAccess`
- `IVideoItemDataMapper`
- `IVideoListDataModelMapper`
- `IAccountRepository`




