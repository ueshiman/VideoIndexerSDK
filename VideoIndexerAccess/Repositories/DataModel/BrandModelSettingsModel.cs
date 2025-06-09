namespace VideoIndexerAccess.Repositories.DataModel
{
    public class BrandModelSettingsModel
    {        /// <summary>
        /// モデル設定の状態（有効/無効）
        /// </summary>
        public bool state { get; set; }

        /// <summary>
        /// ビルトインブランドを使用するかどうか
        /// </summary>
        public bool useBuiltIn { get; set; }
    }
}
