using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            string? location = account.Location;
            if (string.IsNullOrEmpty(location))
            {
                _logger?.LogError("Account.Location is not found.");
                throw new ApplicationException("Account.Location is not found.");
            }

            string? accountId = account.Properties?.Id;
            if (string.IsNullOrEmpty(accountId))
            {
                _logger?.LogError("Account.Properties.Id is not found.");
                throw new ApplicationException("Account.Properties.Id is not found.");
            }
        }
    }
}
