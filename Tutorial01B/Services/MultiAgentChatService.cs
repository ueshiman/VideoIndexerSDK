using Microsoft.Extensions.Logging;

namespace Tutorial01B.Services;

/// <summary>
/// <see cref="AgentOrchestrator"/> を使用して複数エージェントの応答を集約し出力するチャットサービス。
/// </summary>
public sealed class MultiAgentChatService : IChatService
{
    private readonly AgentOrchestrator _orchestrator;
    private readonly ILogger<MultiAgentChatService> _logger;

    public MultiAgentChatService(
        AgentOrchestrator orchestrator,
        ILogger<MultiAgentChatService> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
    }

    public async Task RunSampleAsync(CancellationToken cancellationToken = default)
    {
        const string userMessage = "What's the best way to train a parrot?";

        _logger.LogInformation("Running MultiAgentChatService with message: {Message}", userMessage);

        IReadOnlyDictionary<string, string> results =
            await _orchestrator.HandleAsync(userMessage, cancellationToken);

        foreach (var (agentName, response) in results)
        {
            Console.WriteLine($"=== {agentName} ===");
            Console.WriteLine(response);
            Console.WriteLine();
        }
    }
}
