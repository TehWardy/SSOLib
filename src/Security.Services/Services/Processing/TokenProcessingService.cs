using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;

namespace Security.Services.Processing
{
    public class TokenProcessingService : ITokenProcessingService
    {
        private readonly ITokenService tokenService;

        public TokenProcessingService(ITokenService tokenService)
            => this.tokenService = tokenService;

        public async ValueTask<Token> AddTokenForUserIdAsync(string userId)
            => await tokenService.AddTokenAsync(userId);

        public async ValueTask DeleteTokenAsync(string tokenId)
        { 
            var token = tokenService.GetAllTokens(ignoreFilters: true)
                .FirstOrDefault(t => t.Id == tokenId);

            if(token != null)
                await tokenService.DeleteTokenAsync(token); 
        }

        public Token GetTokenById(string id)
        {
            var token = tokenService.GetAllTokens()
                .FirstOrDefault(t => t.Id == id && t.Expires >= DateTimeOffset.Now);

            if (token == null)
                throw new SecurityException("Access Denied!");

            return token;
        }

        public async ValueTask<Token> GenerateConfirmationToken(string userId)
            => await tokenService.AddTokenAsync(userId, (int)TokenUse.Confirmation);

        public async ValueTask<Token> GenerateForgottenPasswordToken(string userId)
            => await tokenService.AddTokenAsync(userId, (int)TokenUse.PasswordReset);

        public Token GetForgottenPasswordToken(string tokenId)
        {
            int reasonCode = (int)TokenUse.PasswordReset;

            return tokenService.GetAllTokens(ignoreFilters: true)
                .FirstOrDefault(r => r.Expires >= DateTimeOffset.Now && r.Reason == reasonCode && r.Id == tokenId);
        }

        public Token GetConfirmationToken(string tokenId)
        {
            int reasonCode = (int)TokenUse.Confirmation;

            return tokenService.GetAllTokens(ignoreFilters: true)
                .FirstOrDefault(r => r.Expires >= DateTimeOffset.Now && r.Reason == reasonCode && r.Id == tokenId);
        }
    }
}