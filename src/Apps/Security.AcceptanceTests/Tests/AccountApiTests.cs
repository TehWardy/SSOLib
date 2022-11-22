using Bogus;
using Security.AcceptanceTests.Clients;
using Security.Objects.DTOs;
using System.Threading.Tasks;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    [Collection(nameof(AccountTestCollection))]
    public partial class AccountApiTests
    {
        private readonly AuthenticationApiClient authenticationApiClient;
        private readonly UserApiClient userApiClient;

        public AccountApiTests(AuthenticationApiClient authenticationApiClient, UserApiClient userApiClient)
        {
            this.authenticationApiClient = authenticationApiClient;
            this.userApiClient = userApiClient;
        }

        static Auth RandomAuth(RegisterUser user)
            => new Auth()
            {
                User = user.Email,
                Pass = user.Password
            };

        static RegisterUser RandomRegisterUser()
            => GetRegisterUserFiller().Generate();

        static Faker<RegisterUser> GetRegisterUserFiller()
        {
            var filler = new Faker<RegisterUser>()
                .RuleFor(r => r.DisplayName, f => f.Name.FullName())
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .RuleFor(r => r.Password, f => f.Internet.Password() + f.Random.Number(5) + "!")
                .RuleFor(r => r.Culture, f => f.Locale)
                .RuleFor(r => r.PhoneNumber, f => f.Phone.PhoneNumber());

            return filler;
        }

        async Task TearDownUserAsync(string userId)
            => await userApiClient.TearDown(userId);
    }
}
