# VideoIndexerSDK
Azure AI Video Indexer API SDK for C#

---

## Tutorial01B — マルチエージェントチャットサンプル

`Tutorial01B` は、複数の AI エージェントが協調して動作するマルチエージェントチャットシステムのサンプルプロジェクトです。  
エージェントごとのシステムプロンプトを **`promptssettings.json`** で一元管理する設計を採用しています。

### ディレクトリ構成

```
Tutorial01B/
├── appsettings.json          # OpenAI 接続情報・有効エージェント一覧
├── promptssettings.json      # ★ エージェント別システムプロンプト定義
├── Program.cs                # エントリポイント（両設定ファイルを読み込む）
├── Models/
│   ├── AgentSettings.cs      # Agents セクションのマッピングモデル
│   ├── OpenAISettings.cs     # OpenAI セクションのマッピングモデル
│   └── PromptSettings.cs     # Prompts セクションのマッピングモデル
├── Agents/
│   ├── IAgent.cs             # エージェント共通インターフェース
│   ├── SummaryAgent.cs       # 日本語要約エージェント
│   └── TranslationAgent.cs  # 英日・日英翻訳エージェント
├── Executors/
│   ├── IChatCompletionExecutor.cs          # チャット補完 API 抽象化
│   ├── ChatCompletionResult.cs             # 補完結果モデル
│   └── AzureOpenAIChatCompletionExecutor.cs # Azure OpenAI 実装
├── Services/
│   ├── IChatService.cs           # チャットサービスインターフェース
│   ├── AgentOrchestrator.cs      # エージェント協調オーケストレーター
│   └── MultiAgentChatService.cs  # マルチエージェントサービス実装
└── Extensions/
    └── ServiceCollectionExtensions.cs  # DI 登録拡張メソッド
```

### promptssettings.json の管理

エージェントが使用するシステムプロンプトは、`Tutorial01B/promptssettings.json` で管理します。  
`appsettings.json` にはプロンプト定義を含めず、接続情報や有効エージェントの設定のみを記述します。

#### promptssettings.json の例

```json
{
  "Prompts": {
    "SummaryAgent": "You are a helpful assistant that summarizes text concisely in Japanese.",
    "TranslationAgent": "You are a translation assistant. Translate the given Japanese text into English. If the input is already in English, translate it into Japanese instead."
  }
}
```

- **キー**: エージェントクラス名（例: `"SummaryAgent"`）
- **値**: そのエージェントに渡すシステムプロンプト文字列
- キーに対応するエントリが存在しない場合、各エージェントのデフォルトプロンプトにフォールバックします

#### 設計ポイント

| 設定ファイル | 管理内容 |
|---|---|
| `appsettings.json` | OpenAI 接続情報、有効エージェント一覧 |
| `promptssettings.json` | エージェント別システムプロンプト |

`Program.cs` はアプリ起動時に両ファイルを `IConfiguration` に読み込みます。

```csharp
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("promptssettings.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();
```

各エージェントは `IOptions<PromptSettings>` 経由でプロンプトを取得します。

```csharp
public SummaryAgent(IChatCompletionExecutor executor, IOptions<PromptSettings> promptOptions)
{
    _systemPrompt = promptOptions.Value.Prompts.TryGetValue(nameof(SummaryAgent), out var prompt)
        ? prompt
        : DefaultPrompt; // フォールバック
}
```

### 有効エージェントの切り替え

`appsettings.json` の `Agents.Enabled` でエージェントを選択的に有効化できます。

```json
{
  "Agents": {
    "Enabled": [ "SummaryAgent", "TranslationAgent" ]
  }
}
```

- リストが空の場合はすべてのエージェントが有効になります
- エージェント名は大文字・小文字を区別しません

### セットアップ

1. `Tutorial01B/appsettings.json` の `OpenAI` セクションに Azure OpenAI の接続情報を設定します
2. 必要に応じて `Tutorial01B/promptssettings.json` のプロンプトをカスタマイズします
3. プロジェクトをビルド・実行します

```bash
cd Tutorial01B
dotnet run
```

### テスト

`Tutorial01BTests` プロジェクトに単体テストが含まれています。

```bash
cd Tutorial01BTests
dotnet test
```
