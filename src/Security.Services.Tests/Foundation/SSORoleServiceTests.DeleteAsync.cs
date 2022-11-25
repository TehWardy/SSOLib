using System;
using Force.DeepCloner;
using Moq;
using Security.Objects.Entities;
using Xunit;

namespace Security.Services.Tests.Foundation
{
    public partial class SSORoleServiceTests
    {
        [Fact]
        public async void ShouldDeleteSSORoleAsync()
        {
            // given
            SSORole inputSSORole = RandomRole(Guid.NewGuid());
            SSORole expectedSSORole = inputSSORole.DeepClone();

            // when
            await roleService.DeleteSSORoleAsync(inputSSORole);

            // then
            roleBrokerMock.Verify(broker => 
                broker.DeleteSSORoleAsync(inputSSORole), 
                Times.Once);

            roleBrokerMock.VerifyNoOtherCalls();
        }
    }
}