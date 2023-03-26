using Security.Api.Interfaces;
using Security.Objects.DTOs;
using Security.Objects.Entities;
using Security.Services.Orchestration.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Threading.Tasks;

namespace Security.Api
{
    public class AccountManager : IAccountManager
    {
        private readonly IAuthenticationOrchestrationService authService;
        private readonly ISSOUserOrchestrationService registrationService;
        private readonly ISSOUserProcessingService userService;

        public AccountManager(
            IAuthenticationOrchestrationService authService,
            ISSOUserOrchestrationService registrationService,
            ISSOUserProcessingService userService)
        {
            this.authService = authService;
            this.registrationService = registrationService;
            this.userService = userService;
        }

        public ValueTask ChangePasswordAsync(string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async ValueTask ConfirmForgotPasswordAsync(string token, string newPassword) =>
            await registrationService.ConfirmForgotPassword(token, newPassword);
        
        public async ValueTask ConfirmRegistrationAsync(string token) =>
            await registrationService.ConfirmRegistration(token);

        public ValueTask ForgotPasswordAsync(string email, int appId)
        {
            throw new System.NotImplementedException();
        }

        public async ValueTask<Token> LoginAsync(string username, string password) =>
            await authService.LoginAsync(username, password);

        public async ValueTask LogoutAsync() =>
            await authService.Logout();

        public SSOUser Me() =>
            userService.Me();
        
        public async ValueTask<SSOUser> RegisterAsync(RegisterUser registerForm) =>
            await registrationService.Register(registerForm);
    }
}