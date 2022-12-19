using Security.AcceptanceTests.Clients;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    [CollectionDefinition(nameof(AccountTestCollection))]
    public class AccountTestCollection :
        ICollectionFixture<AccountApiClient>,
        ICollectionFixture<RegisterApiClient>,
        ICollectionFixture<SSOUserApiClient>
    {

    }
}
