using B2B.AcceptanceTests.Masterdata.Clients;
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

    [CollectionDefinition(nameof(AuthTestCollection))]
    public class AuthTestCollection :
        ICollectionFixture<AccountApiClient>,
        ICollectionFixture<RegisterApiClient>,
        ICollectionFixture<SSOUserApiClient>
    {

    }

    [CollectionDefinition(nameof(TenantTestCollection))]
    public class TenantTestCollection : 
        ICollectionFixture<TenantApiClient>
    {

    }
}
