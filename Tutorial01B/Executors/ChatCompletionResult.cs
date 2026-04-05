namespace Tutorial01B.Executors;

/// <summary>
/// チャット補完 API のレスポンスを表すモデル。
/// </summary>
public sealed class ChatCompletionResult
{
    /// <summary>補完によって生成されたテキスト。</summary>
    public string Text { get; }

    public ChatCompletionResult(string text)
    {
        Text = text;
    }
}
