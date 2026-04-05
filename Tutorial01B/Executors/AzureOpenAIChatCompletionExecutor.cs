using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using Tutorial01B.Models;

namespace Tutorial01B.Executors;

/// <summary>
/// Azure OpenAI を使用するチャット補完エグゼキューター。
/// appsettings.json の "OpenAI" セクションから接続情報を読み込みます。
/// </summary>
public sealed class AzureOpenAIChatCompletionExecutor : IChatCompletionExecutor
{
    private readonly ChatClient _chatClient;

    public AzureOpenAIChatCompletionExecutor(IOptions<OpenAISettings> options)
    {
        var settings = options.Value;
        var client = new AzureOpenAIClient(
            new Uri(settings.Endpoint),
            new AzureKeyCredential(settings.ApiKey));
        _chatClient = client.GetChatClient(settings.DeploymentName);
    }

    public async Task<ChatCompletionResult> CompleteAsync(
        string systemPrompt,
        string userMessage,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ChatMessage> messages =
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        ];

        var response = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
        var text = response.Value.Content[0].Text;
        return new ChatCompletionResult(text);
    }
}
