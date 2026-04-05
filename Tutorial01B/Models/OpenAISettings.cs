namespace Tutorial01B.Models;

/// <summary>
/// appsettings.json の "OpenAI" セクションにマッピングされるモデル。
/// </summary>
public sealed class OpenAISettings
{
    public const string SectionName = "OpenAI";

    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
}
