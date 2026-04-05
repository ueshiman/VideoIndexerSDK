namespace Tutorial01B.Models;

/// <summary>
/// promptssettings.json の "Prompts" セクションにマッピングされるモデル。
/// エージェント名をキー、システムプロンプト文字列を値として保持します。
/// </summary>
public sealed class PromptSettings
{
    public const string SectionName = "Prompts";

    /// <summary>
    /// エージェント名とシステムプロンプトの辞書。
    /// キーはエージェント名（例: "SummaryAgent"）、値はシステムプロンプト文字列。
    /// </summary>
    public Dictionary<string, string> Prompts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
