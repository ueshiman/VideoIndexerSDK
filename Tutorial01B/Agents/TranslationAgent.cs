using Microsoft.Extensions.Options;
using Tutorial01B.Executors;
using Tutorial01B.Models;

namespace Tutorial01B.Agents;

/// <summary>
/// 日本語↔英語の自動翻訳を行うエージェント。
/// システムプロンプトは promptssettings.json の "Prompts:TranslationAgent" から取得します。
/// </summary>
public sealed class TranslationAgent : IAgent
{
    private const string DefaultPrompt =
        "You are a translation assistant. Translate the given Japanese text into English. " +
        "If the input is already in English, translate it into Japanese instead.";

    private readonly IChatCompletionExecutor _executor;
    private readonly string _systemPrompt;

    public TranslationAgent(IChatCompletionExecutor executor, IOptions<PromptSettings> promptOptions)
    {
        _executor = executor;
        _systemPrompt = promptOptions.Value.Prompts.TryGetValue(nameof(TranslationAgent), out var prompt)
            ? prompt
            : DefaultPrompt;
    }

    public string Name => nameof(TranslationAgent);

    public async Task<string> ReplyAsync(string input, CancellationToken cancellationToken = default)
    {
        var result = await _executor.CompleteAsync(_systemPrompt, input, cancellationToken);
        return result.Text;
    }
}
