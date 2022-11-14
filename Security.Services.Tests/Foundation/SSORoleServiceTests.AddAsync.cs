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
        public async void ShouldAddSSORoleAsync()
        {
            // given
            SSORole inputSSORole = RandomRole(Guid.NewGuid());
            SSORole expectedSSORole = inputSSORole.DeepClone();

            roleBrokerMock.Setup(broker => broker.GetAllSSORoles()).Returns(Array.Empty<SSORole>().AsQueryable());
            roleBrokerMock.Setup(broker => broker.AddSSORoleAsync(inputSSORole)).ReturnsAsync(expectedSSORole);

            // when
            SSORole actualSSORole = await roleService.AddSSORoleAsync(inputSSORole);

            // then
            actualSSORole.Should().BeEquivalentTo(expectedSSORole);

            roleBrokerMock.Verify(broker => 
                broker.AddSSORoleAsync(inputSSORole), 
                Times.Once);

            roleBrokerMock.VerifyNoOtherCalls();
        }
    }
}

