namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// 動画・プロジェクト検索のためのリクエストパラメータモデル。
    /// </summary>
    public class SearchVideosResponseModel
    {
        /// <summary>
        /// ソース言語が一致する動画・プロジェクトのみを含めます。  
        /// 例: en-US、ja-JP、fr-FR など
        /// 
        /// ソース言語が一致する動画やプロジェクトのみを含めます。  
        /// 対応する言語コード一覧（例: ja-JP は日本語）：
        ///  
        /// ar-AE: アラビア語（アラブ首長国連邦）  
        /// ar-BH: アラビア語（バーレーン）  
        /// ar-EG: アラビア語（エジプト）  
        /// ar-IL: アラビア語（イスラエル）  
        /// ar-IQ: アラビア語（イラク）  
        /// ar-JO: アラビア語（ヨルダン）  
        /// ar-KW: アラビア語（クウェート）  
        /// ar-LB: アラビア語（レバノン）  
        /// ar-OM: アラビア語（オマーン）  
        /// ar-PS: アラビア語（パレスチナ）  
        /// ar-QA: アラビア語（カタール）  
        /// ar-SA: アラビア語（サウジアラビア）  
        /// ar-SY: アラビア語（シリア）  
        /// bg-BG: ブルガリア語  
        /// ca-ES: カタルーニャ語  
        /// cs-CZ: チェコ語  
        /// da-DK: デンマーク語  
        /// de-DE: ドイツ語  
        /// el-GR: ギリシャ語  
        /// en-AU: 英語（オーストラリア）  
        /// en-GB: 英語（イギリス）  
        /// en-US: 英語（アメリカ）  
        /// es-ES: スペイン語（スペイン）  
        /// es-MX: スペイン語（メキシコ）  
        /// et-EE: エストニア語  
        /// fa-IR: ペルシャ語  
        /// fi-FI: フィンランド語  
        /// fil-PH: フィリピン語  
        /// fr-CA: フランス語（カナダ）  
        /// fr-FR: フランス語  
        /// ga-IE: アイルランド語  
        /// gu-IN: グジャラート語  
        /// he-IL: ヘブライ語  
        /// hi-IN: ヒンディー語  
        /// hr-HR: クロアチア語  
        /// hu-HU: ハンガリー語  
        /// hy-AM: アルメニア語  
        /// id-ID: インドネシア語  
        /// is-IS: アイスランド語  
        /// it-IT: イタリア語  
        /// ja-JP: 日本語  
        /// kn-IN: カンナダ語  
        /// ko-KR: 韓国語  
        /// lt-LT: リトアニア語  
        /// lv-LV: ラトビア語  
        /// ml-IN: マラヤーラム語  
        /// ms-MY: マレー語  
        /// nb-NO: ノルウェー語  
        /// nl-NL: オランダ語  
        /// pl-PL: ポーランド語  
        /// pt-BR: ポルトガル語（ブラジル）  
        /// pt-PT: ポルトガル語（ポルトガル）  
        /// ro-RO: ルーマニア語  
        /// ru-RU: ロシア語  
        /// sk-SK: スロバキア語  
        /// sl-SI: スロベニア語  
        /// sv-SE: スウェーデン語  
        /// ta-IN: タミル語  
        /// te-IN: テルグ語  
        /// th-TH: タイ語  
        /// tr-TR: トルコ語  
        /// uk-UA: ウクライナ語  
        /// vi-VN: ベトナム語  
        /// zh-Hans: 中国語（簡体字）  
        /// zh-HK: 中国語（広東語、繁体字）  
        /// </summary>
        public string? SourceLanguage { get; set; }

        /// <summary>
        /// trueの場合はソース動画ファイルを持つ動画を含みます。  
        /// falseの場合はプロジェクトやソースのない動画も含まれます。
        /// </summary>
        public bool? HasSourceVideoFile { get; set; }

        /// <summary>
        /// 指定された動画IDに一致する動画、またはその動画を含むプロジェクトのみを対象とします。
        /// </summary>
        public string? SourceVideoId { get; set; }

        /// <summary>
        /// 処理状態でフィルターします。プロジェクトは常に「Processed」状態です。  
        /// 許可される値: Uploaded, Processing, Processed, Failed
        /// </summary>
        public string[]? State { get; set; }

        /// <summary>
        /// プライバシーレベルでフィルターします。  
        /// 許可される値: Private, Public
        /// </summary>
        public string[]? Privacy { get; set; }

        /// <summary>
        /// 検索対象とする動画IDを指定します。
        /// </summary>
        public string[]? Id { get; set; }

        /// <summary>
        /// 検索対象とするパーティションを指定します。
        /// </summary>
        public string[]? Partition { get; set; }

        /// <summary>
        /// アップロード時に紐付けた外部IDで検索します。
        /// </summary>
        public string[]? ExternalId { get; set; }

        /// <summary>
        /// 動画の所有者で検索します。
        /// </summary>
        public string[]? Owner { get; set; }

        /// <summary>
        /// 検出された顔に基づいて検索します。
        /// </summary>
        public string[]? Face { get; set; }

        /// <summary>
        /// 検出されたアニメキャラクターに基づいて検索します。
        /// </summary>
        public string[]? AnimatedCharacter { get; set; }

        /// <summary>
        /// フリーテキストで動画を検索します。  
        /// 例:  
        /// &query=north america → "north" または "america" を含む動画  
        /// &query=north+america → 両方の単語を含む動画  
        /// &query="north america" → 完全一致のフレーズ検索  
        /// ※クエリはURLエンコードしてください。
        /// </summary>
        public string[]? Query { get; set; }

        /// <summary>
        /// テキスト検索のスコープを指定します。  
        /// 許可される値: Transcript, Topics, Ocr, Annotations, Brands, NamedLocations, NamedPeople, Name, DetectedObjects, Metadata
        /// </summary>
        public string[]? TextScope { get; set; }

        /// <summary>
        /// 検索対象の言語を指定します。複数指定可能。  
        /// 指定がない場合、すべての言語で検索されます。
        /// </summary>
        public string[]? Language { get; set; }

        /// <summary>
        /// 指定した日付より後に作成されたアイテムを検索します。  
        /// 形式: RFC 3339（例: 2017-07-21T17:32:28Z）
        /// </summary>
        public string? CreatedAfter { get; set; }

        /// <summary>
        /// 指定した日付より前に作成されたアイテムを検索します。  
        /// 形式: RFC 3339（例: 2017-07-21T17:32:28Z）
        /// </summary>
        public string? CreatedBefore { get; set; }
    }
}
