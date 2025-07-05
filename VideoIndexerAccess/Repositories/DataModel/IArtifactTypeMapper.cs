namespace VideoIndexerAccess.Repositories.DataModel;

public interface IArtifactTypeMapper
{
    /// <summary>
    /// 指定されたメンバー名から <see cref="ArtifactType"/> へのマッピングを試みます。
    /// </summary>
    /// <param name="memberName">マッピングするメンバー名。</param>
    /// <param name="artifactType">マッピングされた <see cref="ArtifactType"/>。失敗した場合は既定値。</param>
    /// <returns>マッピングに成功した場合は true、それ以外は false。</returns>
    bool TryMap(string memberName, out ArtifactType artifactType);

    /// <summary>
    /// 指定されたメンバー名から <see cref="ArtifactType"/> へマッピングします。
    /// マッピングに失敗した場合は <see cref="ArgumentException"/> をスローします。
    /// </summary>
    /// <param name="memberName">マッピングするメンバー名。</param>
    /// <returns>マッピングされた <see cref="ArtifactType"/>。</returns>
    /// <exception cref="ArgumentException">無効なメンバー名が指定された場合。</exception>
    ArtifactType MapFrom(string memberName);

    /// <summary>
    /// 指定された <see cref="ArtifactType"/> を文字列に変換します。
    /// 無効な値が指定された場合は <see cref="ArgumentException"/> をスローします。
    /// </summary>
    /// <param name="artifactType">変換する <see cref="ArtifactType"/>。</param>
    /// <returns>文字列に変換された <see cref="ArtifactType"/>。</returns>
    /// <exception cref="ArgumentException">無効な <see cref="ArtifactType"/> 値が指定された場合。</exception>
    string MapToString(ArtifactType artifactType);
}