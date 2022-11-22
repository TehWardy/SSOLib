using Security.Objects.DTOs;
using Security.Objects.Entities;
using Security.Services.Services.Orchestration.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;

namespace Security.Services.Services.Orchestration
{
    public class SSOUserOrchestrationService : ISSOUserOrchestrationService
    {
        readonly ISSOUserProcessingService ssoUserProcessingService;
        readonly ITokenProcessingService tokenProcessingService;
        readonly ISessionProcessingService sessionService;

        public SSOUserOrchestrationService(
            ISSOUserProcessingService ssoUserProcessingService,
            ITokenProcessingService tokenProcessingService,
            ISessionProcessingService sessionService)
        {
            this.ssoUserProcessingService = ssoUserProcessingService;
            this.tokenProcessingService = tokenProcessingService;
            this.sessionService = sessionService;
        }

        public SSOUser GetUserByTokenId(string tokenId)
            => ssoUserProcessingService.FindByTokenId(tokenId);

        public SSOUser GetSSOUserById(string id)
        {
            var user = ssoUserProcessingService
                .GetAllSSOUsers()
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new SecurityException("Access Denied.");

            return user;
        }

        public SSOUser GetSSOUserFromSession()
            => sessionService.GetUser();

        public async ValueTask<SSOUser> Register(RegisterUser registerForm)
        {
            ValidateRegisterForm(registerForm);

            var mappedUser = MapToSSOUser(registerForm);

            return await ssoUserProcessingService.RegisterSSOUserAsync(mappedUser);
        }

        static void ValidateRegisterForm(RegisterUser registerForm)
        {
            if (!registerForm.Email.Contains('@'))
                throw new Exception("Invalid email provided");

            if (string.IsNullOrEmpty(registerForm.DisplayName))
                throw new Exception("Display name cannot be empty");

            if (string.IsNullOrEmpty(registerForm.Password))
                throw new Exception("Password cannot be empty");
        }

        private SSOUser MapToSSOUser(RegisterUser registerForm)
            => new SSOUser
            {
                Id = registerForm.Email.Split("@")[0],
                DisplayName = registerForm.DisplayName,
                PasswordHash = registerForm.Password,
                Email = registerForm.Email,
                PhoneNumber = registerForm.PhoneNumber
            };

        public async ValueTask ChangePassword(string oldPassword, string newPassword)
        {
            var user = sessionService.GetUser();
            ssoUserProcessingService.FindByUserAndPassword(user.Id, oldPassword);
            user.PasswordHash = newPassword;
            await ssoUserProcessingService.UpdateSSOUserAsync(user);
        }

        public async ValueTask ConfirmForgotPassword(string tokenId, string newPassword)
        {
            var user = ValidateConfirmationToken(tokenId);
            user.PasswordHash = newPassword;
            await ssoUserProcessingService.UpdateSSOUserAsync(user);
        }

        public async ValueTask ConfirmRegistration(string tokenId)
        {
            var user = ValidateConfirmationToken(tokenId);
            user.EmailConfirmed = true;
            await ssoUserProcessingService.UpdateSSOUserAsync(user);
        }

        SSOUser ValidateConfirmationToken(string tokenId)
        {
            var token = tokenProcessingService.GetTokenById(tokenId);

            if (token == null || token.Expires < DateTimeOffset.UtcNow)
                throw new SecurityException("Access Denied!");

            var user = ssoUserProcessingService
                .GetAllSSOUsers()
                .FirstOrDefault(u => u.Id == token.UserName);

            if (user == null)
                throw new SecurityException("Access Denied!");

            return user;
        }
    }
}