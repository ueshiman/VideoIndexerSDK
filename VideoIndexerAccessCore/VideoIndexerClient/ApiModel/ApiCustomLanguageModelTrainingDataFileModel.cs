namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

/// <summary>
/// カスタム言語モデルのトレーニングデータファイルを表すクラス
/// </summary>
public class ApiCustomLanguageModelTrainingDataFileModel
{
    public string? id { get; set; }
    public string? name { get; set; }
    public bool? enable { get; set; }
    public string? creator { get; set; }
    public string? creationTime { get; set; }
}
