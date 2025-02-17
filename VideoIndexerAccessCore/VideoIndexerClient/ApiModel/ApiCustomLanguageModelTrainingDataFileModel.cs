namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

/// <summary>
/// カスタム言語モデルのトレーニングデータファイルを表すクラス
/// </summary>
public class ApiCustomLanguageModelTrainingDataFileModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Enable { get; set; }
    public string Creator { get; set; } = string.Empty;
    public string CreationTime { get; set; } = string.Empty;
}