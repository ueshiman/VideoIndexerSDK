using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Testing.Platform.Logging;
using Moq;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCoreTests.VideoIndexerClient.ApiAccess
{
    [TestClass]
    public class AccountMigrationStatusTests
    {
        private Microsoft.Extensions.Logging.ILogger<AccountMigrationStatusApiAccess> _loggerMock;
        private Mock<IDurableHttpClient> _durableHttpClientMock;
        //private Mock<IAccountTokenProviderDynamic> _accountTokenProviderMock;
        private Mock<IApiResourceConfigurations> _apiResourceConfigurationsMock;
        private AccountMigrationStatusApiAccess _accountMigrationStatus;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new NullLogger<AccountMigrationStatusApiAccess>();
            _durableHttpClientMock = new Mock<IDurableHttpClient>();
            //_accountTokenProviderMock = new Mock<IAccountTokenProviderDynamic>();
            _apiResourceConfigurationsMock = new Mock<IApiResourceConfigurations>();
            _apiResourceConfigurationsMock.SetupGet(x => x.ApiEndpoint).Returns("https://api.example.com");

            _accountMigrationStatus = new AccountMigrationStatusApiAccess(
                _loggerMock,
                _durableHttpClientMock.Object,
                //_accountTokenProviderMock.Object,
                _apiResourceConfigurationsMock.Object
            );
        }

        // ... (rest of the code remains unchanged)
    }
}