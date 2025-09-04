using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ArtifactTypeMapper : IArtifactTypeMapper
    {
        private static readonly Dictionary<string, ArtifactType> NameToType = Enum.GetValues<ArtifactType>().ToDictionary(e => e.ToString(), e => e, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 指定されたメンバー名から <see cref="ArtifactType"/> へのマッピングを試みます。
        /// </summary>
        /// <param name="memberName">マッピングするメンバー名。</param>
        /// <param name="artifactType">マッピングされた <see cref="ArtifactType"/>。失敗した場合は既定値。</param>
        /// <returns>マッピングに成功した場合は true、それ以外は false。</returns>
        public bool TryMap(string memberName, out ArtifactType artifactType)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                artifactType = default;
                return false;
            }

            return NameToType.TryGetValue(memberName, out artifactType);
        }

        /// <summary>
        /// 指定されたメンバー名から <see cref="ArtifactType"/> へマッピングします。
        /// マッピングに失敗した場合は <see cref="ArgumentException"/> をスローします。
        /// </summary>
        /// <param name="memberName">マッピングするメンバー名。</param>
        /// <returns>マッピングされた <see cref="ArtifactType"/>。</returns>
        /// <exception cref="ArgumentException">無効なメンバー名が指定された場合。</exception>
        public ArtifactType MapFrom(string memberName) => TryMap(memberName, out var artifactType) ? artifactType : throw new ArgumentException($"Invalid ArtifactType name: {memberName}", nameof(memberName));

        /// <summary>
        /// 指定された <see cref="ArtifactType"/> を文字列に変換します。
        /// 無効な値が指定された場合は <see cref="ArgumentException"/> をスローします。
        /// </summary>
        /// <param name="artifactType">変換する <see cref="ArtifactType"/>。</param>
        /// <returns>文字列に変換された <see cref="ArtifactType"/>。</returns>
        /// <exception cref="ArgumentException">無効な <see cref="ArtifactType"/> 値が指定された場合。</exception>
        public string MapToString(ArtifactType artifactType)
        {
            if (!Enum.IsDefined(typeof(ArtifactType), artifactType)) throw new ArgumentException($"Invalid ArtifactType value: {artifactType}", nameof(artifactType));
            return artifactType.ToString();
        }
    }
}
