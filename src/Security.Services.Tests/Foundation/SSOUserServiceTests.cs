using System;
using System.Linq;
using FizzWare.NBuilder;
using Moq;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserServiceTests
    {
        readonly Mock<ISSOUserBroker> userBrokerMock;
        readonly ISSOUserService userService;

        public SSOUserServiceTests()
        {
            userBrokerMock = new Mock<ISSOUserBroker>();
            userService = new SSOUserService(userBrokerMock.Object);
        }

        static string RandomString()
            => new RandomGenerator().NextString(5, 12);

        static IQueryable<SSOUser> RandomUsers()
            => Enumerable.Range(0, new Random().Next(100))
                .Select(i => RandomUser(RandomString()))
                .AsQueryable();

        static SSOUser RandomUser(string id)
            => Builder<SSOUser>
                .CreateNew()
                .With(i => i.Id = id)
                .Build();
    }
}