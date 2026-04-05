using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorial01B.Agents;
using Tutorial01B.Executors;
using Tutorial01B.Models;
using Tutorial01B.Services;

namespace Tutorial01B.Extensions;

/// <summary>
/// DI サービス登録用の拡張メソッドを提供します。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// マルチエージェントシステム全体（エグゼキューター・エージェント・オーケストレーター・チャットサービス）を
    /// DI コンテナに登録します。
    /// </summary>
    public static IServiceCollection AddMultiAgentSystem(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 設定モデルの登録
        services.Configure<OpenAISettings>(configuration.GetSection(OpenAISettings.SectionName));
        services.Configure<AgentSettings>(configuration.GetSection(AgentSettings.SectionName));
        services.Configure<PromptSettings>(configuration.GetSection(PromptSettings.SectionName));

        // Azure OpenAI エグゼキューターの登録
        services.AddSingleton<IChatCompletionExecutor, AzureOpenAIChatCompletionExecutor>();

        // 有効なエージェントのみを登録
        var agentSettings = configuration
            .GetSection(AgentSettings.SectionName)
            .Get<AgentSettings>() ?? new AgentSettings();

        bool IsEnabled(string agentName) =>
            agentSettings.Enabled.Count == 0 ||
            agentSettings.Enabled.Contains(agentName, StringComparer.OrdinalIgnoreCase);

        if (IsEnabled(nameof(SummaryAgent)))
            services.AddSingleton<IAgent, SummaryAgent>();

        if (IsEnabled(nameof(TranslationAgent)))
            services.AddSingleton<IAgent, TranslationAgent>();

        // オーケストレーターとチャットサービスの登録
        services.AddSingleton<AgentOrchestrator>();
        services.AddSingleton<IChatService, MultiAgentChatService>();

        return services;
    }
}
