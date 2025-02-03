namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization;

public interface IAccountTokenProviderDynamic
{
    Task<string> GetArmAccessTokenAsync(CancellationToken ct = default);
    string GetArmAccessToken(CancellationToken ct = default);
    Task<string> GetAccountAccessTokenAsync(string armAccessToken, ArmAccessTokenPermission permission = ArmAccessTokenPermission.Contributor, ArmAccessTokenScope scope = ArmAccessTokenScope.Account, CancellationToken ct = default);
}