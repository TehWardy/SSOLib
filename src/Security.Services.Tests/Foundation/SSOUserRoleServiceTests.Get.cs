using FluentAssertions;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserRoleServiceTests
    {
        [Fact]
        public void ShouldGetSSOUserRolesAsync()
        {
            // given
            IQueryable<SSOUserRole> expectedSSOUserRoles = RandomUserRoles();
            userRoleBrokerMock.Setup(broker => broker.GetAllSSOUserRoles()).Returns(expectedSSOUserRoles);

            // when
            IEnumerable<SSOUserRole> actualSSOUserRoles = userRoleService.GetAllSSOUserRoles();

            // then
            actualSSOUserRoles.Should().BeEquivalentTo(expectedSSOUserRoles);
            userRoleBrokerMock.Verify(broker => broker.GetAllSSOUserRoles(), Times.Once);
            userRoleBrokerMock.VerifyNoOtherCalls();
        }
    }
}