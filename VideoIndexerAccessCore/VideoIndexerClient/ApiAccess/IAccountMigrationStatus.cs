using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IAccountMigrationStatus
{
    Task<AccountMigrationStatusModel?> GetAccountMigrationStatusAsync(string location, string accountId, string accessToken = null);
    AccountMigrationStatusModel? DeserializeResponse(string jsonString);
    Task<string> GetAccountMigrationStatusJsonAsync(string location, string accountId, string? accessToken = null);
}