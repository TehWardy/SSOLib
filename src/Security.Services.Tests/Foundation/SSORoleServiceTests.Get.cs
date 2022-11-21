using FluentAssertions;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSORoleServiceTests
    {
        [Fact]
        public void ShouldGetSSORolesAsync()
        {
            // given
            IQueryable<SSORole> expectedSSORoles = RandomRoles();
            roleBrokerMock.Setup(broker => broker.GetAllSSORoles()).Returns(expectedSSORoles);

            // when
            IEnumerable<SSORole> actualSSORoles = roleService.GetAllSSORoles();

            // then
            actualSSORoles.Should().BeEquivalentTo(expectedSSORoles);
            roleBrokerMock.Verify(broker => broker.GetAllSSORoles(), Times.Once);
            roleBrokerMock.VerifyNoOtherCalls();
        }
    }
}