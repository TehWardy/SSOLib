﻿using Bogus;
using Security.AcceptanceTests.Clients;
using Security.Objects.DTOs;
using System.Threading.Tasks;
using Xunit;

namespace Security.AcceptanceTests.Tests
{
    [Collection(nameof(AccountTestCollection))]
    public partial class AccountApiTests
    {
        private readonly AccountApiClient accountApiClient;
        private readonly RegisterApiClient registerApiClient;
        private readonly SSOUserApiClient ssoUserApiClient;

        public AccountApiTests(AccountApiClient userApiClient, RegisterApiClient registerApiClient, SSOUserApiClient ssoUserApiClient)
        {
            this.accountApiClient = userApiClient;
            this.registerApiClient = registerApiClient;
            this.ssoUserApiClient = ssoUserApiClient;
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
            => await accountApiClient.TearDown(userId);
    }
}
