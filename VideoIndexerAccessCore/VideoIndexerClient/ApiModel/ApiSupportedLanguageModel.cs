namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// サポートされる言語のデータモデル
    /// </summary>
    public class ApiSupportedLanguageModel
    {
        /// <summary> 言語の名前 (例: "English", "日本語") </summary>
        public string name { get; set; } = string.Empty;

        /// <summary> 言語コード (例: "en-US", "ja-JP") </summary>
        public string languageCode { get; set; } = string.Empty;

        /// <summary> 言語が右から左に書かれるかどうか (例: アラビア語は true) </summary>
        public bool isRightToLeft { get; set; }

        /// <summary> この言語がソース言語としてサポートされているか </summary>
        public bool isSourceLanguage { get; set; }

        /// <summary> 自動検出が可能な言語かどうか </summary>
        public bool isAutoDetect { get; set; }

        /// <summary> 言語データセット用にサポートされているか </summary>
        public bool isSupportedForLanguageDataset { get; set; }

        /// <summary> 発音データセット用にサポートされているか </summary>
        public bool isSupportedForPronunciationDataset { get; set; }

        /// <summary> カスタムモデルの作成に対応しているか </summary>
        public bool isSupportedForCustomModels { get; set; }

        /// <summary> 翻訳機能に対応しているか </summary>
        public bool isSupportedForTranslation { get; set; }
    }
}
