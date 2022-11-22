using System.Security;
using Xunit;

namespace Security.Services.Tests.Orchestration
{
    public partial class AuthenticationOrchestrationServiceTests
    {
        [Fact]
        public async void LoginThrowsSecurityExceptionIfNotValidLogin()
        {
            //given
            string username = RandomString();
            string password = RandomString();

            //when & then
            await Assert.ThrowsAsync<SecurityException>(async() => await authenticationOrchestrationService.LoginAsync(username, password));
        }
    }
}
