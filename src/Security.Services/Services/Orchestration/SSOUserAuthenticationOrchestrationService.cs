using Security.Objects.DTOs;
using Security.Objects.Entities;
using Security.Services.Services.Orchestration.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;

namespace Security.Services.Services.Orchestration
{
    public class SSOUserAuthenticationOrchestrationService : ISSOUserAuthenticationOrchestrationService
    {
        readonly ISSOUserProcessingService ssoUserProcessingService;
        readonly ITokenProcessingService tokenProcessingService;
        readonly ISessionProcessingService sessionService;

        public SSOUserAuthenticationOrchestrationService(
            ISSOUserProcessingService ssoUserProcessingService,
            ITokenProcessingService tokenProcessingService,
            ISessionProcessingService sessionService)
        {
            this.ssoUserProcessingService = ssoUserProcessingService;
            this.tokenProcessingService = tokenProcessingService;
            this.sessionService = sessionService;
        }

        public async ValueTask<Token> GenerateConfirmationToken(string userId, int reasonCode)
            => await tokenProcessingService.GenerateConfirmationToken(userId, reasonCode);

        public async ValueTask<Token> Login(string username, string password)
        {
            var user = ssoUserProcessingService.Login(username, password);
            sessionService.SetUser(user);
            var token = await tokenProcessingService.AddTokenForUser(user.Id);
            sessionService.SetString("token", token.Id);
            return token;
        }

        public async ValueTask Logout(string tokenId = null)
        {
            tokenId ??= sessionService.GetString("token");
            sessionService.SetString("token", null);
            sessionService.SetUser(null);
            await tokenProcessingService.DeleteTokenAsync(tokenId);
        }

        public SSOUser GetUserByTokenId(string tokenId)
            => ssoUserProcessingService
                .GetAllSSOUsers()
                .FirstOrDefault(u => u.Tokens.Any(t => t.Id == tokenId));

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

            return (await ssoUserProcessingService.RegisterSSOUserAsync(new SSOUser
            {
                Id = registerForm.Email.Split("@")[0],
                DisplayName = registerForm.DisplayName,
                PasswordHash = registerForm.Password,
                Email = registerForm.Email
            }));
        }

        public async ValueTask ChangePassword(string oldPassword, string newPassword)
        {
            var user = sessionService.GetUser();
            ssoUserProcessingService.Login(user.Id, oldPassword);
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

        static void ValidateRegisterForm(RegisterUser registerForm)
        {
            if (!registerForm.Email.Contains('@'))
                throw new Exception("Invalid email provided");

            if (string.IsNullOrEmpty(registerForm.DisplayName))
                throw new Exception("Display name cannot be empty");

            if (string.IsNullOrEmpty(registerForm.Password))
                throw new Exception("Password cannot be empty");
        }
    }
}