using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserRoleServiceTests
    {
        [Fact]
        public async void ShouldAddSSOUserRoleAsync()
        {
            // given
            SSOUserRole inputSSOUserRole = RandomUserRole();
            SSOUserRole expectedSSOUserRole = inputSSOUserRole.DeepClone();

            userRoleBrokerMock.Setup(broker => broker.GetAllSSOUserRoles()).Returns(Array.Empty<SSOUserRole>().AsQueryable());
            userRoleBrokerMock.Setup(broker => broker.AddSSOUserRoleAsync(inputSSOUserRole)).ReturnsAsync(expectedSSOUserRole);

            // when
            SSOUserRole actualSSOUserRole = await userRoleService.AddSSOUserRoleAsync(inputSSOUserRole);

            // then
            actualSSOUserRole.Should().BeEquivalentTo(expectedSSOUserRole);

            userRoleBrokerMock.Verify(broker => 
                broker.AddSSOUserRoleAsync(inputSSOUserRole), 
                Times.Once);

            userRoleBrokerMock.VerifyNoOtherCalls();
        }
    }
}

