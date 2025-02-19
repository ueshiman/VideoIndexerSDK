/// <summary>
/// 言語モデルの編集履歴エントリを表すクラス
/// </summary>
public class ApiLanguageModelEditModel
{
    public int id { get; set; }
    public string videoId { get; set; } = string.Empty;
    public int lineId { get; set; }
    public string createDate { get; set; } = string.Empty;
    public string originalValue { get; set; } = string.Empty;
    public string currentValue { get; set; } = string.Empty;
    public int linguisticTrainingDataGroupsId { get; set; }
    public string videoName { get; set; } = string.Empty;
}
