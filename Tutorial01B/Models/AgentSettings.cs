namespace Tutorial01B.Models;

/// <summary>
/// appsettings.json の "Agents" セクションにマッピングされるモデル。
/// </summary>
public sealed class AgentSettings
{
    public const string SectionName = "Agents";

    /// <summary>
    /// 有効化するエージェント名のリスト。
    /// 空の場合はすべてのエージェントが有効になります。
    /// </summary>
    public IList<string> Enabled { get; set; } = [];
}
