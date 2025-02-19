/// <summary>
/// 言語モデルに関連するファイルのメタデータ
/// </summary>
public class ApiLanguageModelFileMetadataModel
{
    /// <summary>ファイルID（GUID）</summary>
    public string? id { get; set; }

    /// <summary>ファイル名</summary>
    public string? name { get; set; }

    /// <summary>ファイルが有効かどうか</summary>
    public bool enable { get; set; }

    /// <summary>作成者</summary>
    public string? creator { get; set; }

    /// <summary>ファイル作成日時</summary>
    public DateTime? creationTime { get; set; }
}
