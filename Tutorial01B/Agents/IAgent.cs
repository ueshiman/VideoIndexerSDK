namespace Tutorial01B.Agents;

/// <summary>
/// すべてのエージェント共通のインターフェース。
/// </summary>
public interface IAgent
{
    /// <summary>エージェントの名前。</summary>
    string Name { get; }

    /// <summary>
    /// ユーザー入力に対してエージェントが応答を返します。
    /// </summary>
    Task<string> ReplyAsync(string input, CancellationToken cancellationToken = default);
}
