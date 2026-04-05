namespace Tutorial01B.Services;

/// <summary>
/// チャットサービスの共通インターフェース。
/// </summary>
public interface IChatService
{
    Task RunSampleAsync(CancellationToken cancellationToken = default);
}
