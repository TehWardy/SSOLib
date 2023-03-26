using FizzWare.NBuilder;
using Moq;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation;
using Security.Services.Foundation.Interfaces;
using System;
using System.Linq;

namespace Security.Services.Tests.Foundation
{
    public partial class TokenServiceTests
    {
        readonly Mock<ITokenBroker> tokenBrokerMock;
        readonly ITokenService tokenService;

        public TokenServiceTests()
        {
            tokenBrokerMock = new Mock<ITokenBroker>();
            tokenService = new TokenService(tokenBrokerMock.Object, null);
        }

        static string RandomString()
            => new RandomGenerator().NextString(5, 12);

        static IQueryable<Token> RandomTokens()
            => Enumerable.Range(0, new Random().Next(100))
                .Select(i => RandomToken(RandomString()))
                .AsQueryable();

        static Token RandomToken(string id)
            => Builder<Token>
                .CreateNew()
                .With(i => i.Id = id)
                .Build();
    }
}