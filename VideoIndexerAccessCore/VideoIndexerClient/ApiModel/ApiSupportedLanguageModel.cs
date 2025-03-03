namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// サポートされる言語のデータモデル
    /// </summary>
    public class ApiSupportedLanguageModel
    {
        /// <summary> 言語の名前 (例: "English", "日本語") </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> 言語コード (例: "en-US", "ja-JP") </summary>
        public string LanguageCode { get; set; } = string.Empty;

        /// <summary> 言語が右から左に書かれるかどうか (例: アラビア語は true) </summary>
        public bool IsRightToLeft { get; set; }

        /// <summary> この言語がソース言語としてサポートされているか </summary>
        public bool IsSourceLanguage { get; set; }

        /// <summary> 自動検出が可能な言語かどうか </summary>
        public bool IsAutoDetect { get; set; }

        /// <summary> 言語データセット用にサポートされているか </summary>
        public bool IsSupportedForLanguageDataset { get; set; }

        /// <summary> 発音データセット用にサポートされているか </summary>
        public bool IsSupportedForPronunciationDataset { get; set; }

        /// <summary> カスタムモデルの作成に対応しているか </summary>
        public bool IsSupportedForCustomModels { get; set; }

        /// <summary> 翻訳機能に対応しているか </summary>
        public bool IsSupportedForTranslation { get; set; }
    }
}
