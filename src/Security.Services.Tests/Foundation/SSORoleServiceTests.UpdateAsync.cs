using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSORoleServiceTests
    {
        [Fact]
        public async void ShouldUpdateSSORoleAsync()
        {
            // given
            SSORole inputSSORole = RandomRole(Guid.NewGuid());
            SSORole expectedSSORole = inputSSORole.DeepClone();

            roleBrokerMock.Setup(broker => broker.UpdateSSORoleAsync(inputSSORole)).ReturnsAsync(expectedSSORole);

            // when
            SSORole actualSSORole = await roleService.UpdateSSORoleAsync(inputSSORole);

            // then
            actualSSORole.Should().BeEquivalentTo(expectedSSORole);

            roleBrokerMock.Verify(broker => 
                broker.UpdateSSORoleAsync(inputSSORole), 
                Times.Once);

            roleBrokerMock.VerifyNoOtherCalls();
        }
    }
}