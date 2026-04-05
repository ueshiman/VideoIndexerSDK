namespace Tutorial01B.Executors;

/// <summary>
/// チャット補完 API の抽象化インターフェース。
/// </summary>
public interface IChatCompletionExecutor
{
    /// <summary>
    /// システムプロンプトとユーザーメッセージを受け取り、補完結果を返します。
    /// </summary>
    Task<ChatCompletionResult> CompleteAsync(
        string systemPrompt,
        string userMessage,
        CancellationToken cancellationToken = default);
}
