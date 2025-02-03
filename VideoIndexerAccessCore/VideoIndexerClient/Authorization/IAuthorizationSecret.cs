namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization;

public interface IAuthorizationSecret
{
    string TenantId { get; }
    string ClientId { get; }
    string ClientSecret { get; }
}