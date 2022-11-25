using System;
using FluentAssertions;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class TokenServiceTests
    {
        [Fact]
        public async void ShouldAddTokenAsync()
        {
            // given
            string userId = RandomString();
            Token expectedToken = new()
            {
                Id = Guid.NewGuid().ToString().Replace("-", "") + Guid.NewGuid().ToString().Replace("-", ""),
                Expires = DateTimeOffset.Now.AddMinutes(10),
                Reason = 0,
                UserName = userId
            };

            tokenBrokerMock.Setup(broker => broker.AddTokenAsync(It.IsAny<Token>())).ReturnsAsync(expectedToken);

            // when
            Token actualToken = await tokenService.AddTokenAsync(userId);
            expectedToken.Expires = actualToken.Expires;

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
            tokenBrokerMock.Verify(broker => broker.AddTokenAsync(It.IsAny<Token>()), Times.Once);
            tokenBrokerMock.VerifyNoOtherCalls();
        }
    }
}