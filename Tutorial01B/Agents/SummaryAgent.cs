using Microsoft.Extensions.Options;
using Tutorial01B.Executors;
using Tutorial01B.Models;

namespace Tutorial01B.Agents;

/// <summary>
/// ユーザー入力を日本語で要約するエージェント。
/// システムプロンプトは promptssettings.json の "Prompts:SummaryAgent" から取得します。
/// </summary>
public sealed class SummaryAgent : IAgent
{
    private const string DefaultPrompt =
        "You are a helpful assistant that summarizes text concisely in Japanese.";

    private readonly IChatCompletionExecutor _executor;
    private readonly string _systemPrompt;

    public SummaryAgent(IChatCompletionExecutor executor, IOptions<PromptSettings> promptOptions)
    {
        _executor = executor;
        _systemPrompt = promptOptions.Value.Prompts.TryGetValue(nameof(SummaryAgent), out var prompt)
            ? prompt
            : DefaultPrompt;
    }

    public string Name => nameof(SummaryAgent);

    public async Task<string> ReplyAsync(string input, CancellationToken cancellationToken = default)
    {
        var result = await _executor.CompleteAsync(_systemPrompt, input, cancellationToken);
        return result.Text;
    }
}
