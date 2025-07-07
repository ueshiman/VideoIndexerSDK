using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<AccountRepository>? _logger;

        public AccountRepository(ILogger<AccountRepository>? logger)
        {
            _logger = logger;
        }

        public void CheckAccount(ApiAccountModel? account)
        {
            if (account is null)
            {
                _logger?.LogError("Account is not found.");
                throw new ApplicationException("Account is not found.");
            }

            string? location = account.location;
            if (string.IsNullOrEmpty(location))
            {
                _logger?.LogError("Account.location is not found.");
                throw new ApplicationException("Account.location is not found.");
            }

            string? accountId = account.properties?.id;
            if (string.IsNullOrEmpty(accountId))
            {
                _logger?.LogError("Account.properties.id is not found.");
                throw new ApplicationException("Account.properties.id is not found.");
            }
        }
    }
}
