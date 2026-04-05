using Tutorial01B.Agents;

namespace Tutorial01B.Services;

/// <summary>
/// 登録済みのすべてのエージェントを並列または順次に呼び出し、
/// 各エージェントの応答を辞書形式で返すオーケストレーター。
/// </summary>
public sealed class AgentOrchestrator
{
    private readonly IEnumerable<IAgent> _agents;

    public AgentOrchestrator(IEnumerable<IAgent> agents)
    {
        _agents = agents;
    }

    /// <summary>
    /// すべての登録エージェントに対して <paramref name="message"/> を送信し、
    /// 「エージェント名 → 応答」のマッピングを返します。
    /// </summary>
    public async Task<IReadOnlyDictionary<string, string>> HandleAsync(
        string message,
        CancellationToken cancellationToken = default)
    {
        var results = new Dictionary<string, string>();

        foreach (var agent in _agents)
        {
            results[agent.Name] = await agent.ReplyAsync(message, cancellationToken);
        }

        return results;
    }
}
