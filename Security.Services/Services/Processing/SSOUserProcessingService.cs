using Security.Data.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;

namespace Security.Services.Processing
{
    public partial class SSOUserProcessingService : ISSOUserProcessingService
    {
        readonly ISSOUserService ssoUserService;
        readonly ICrypto<string> passwordCrypto;

        public SSOUserProcessingService(ISSOUserService ssoUserService, ICrypto<string> passwordCrypto)
        {         
            this.ssoUserService = ssoUserService;
            this.passwordCrypto = passwordCrypto;
        }

        public async ValueTask<SSOUser> RegisterSSOUserAsync(SSOUser user)
        {
            ValidateSSOUser(user);
            user.PasswordHash = passwordCrypto.Encrypt(user.PasswordHash);
            return await ssoUserService.AddSSOUserAsync(user);
        }

        public async ValueTask DeleteSSOUserAsync(SSOUser item)
            => await ssoUserService.DeleteSSOUserAsync(item);

        public SSOUser Login(string username, string password)
        {
            ValidateUsername(username);

            var user = ssoUserService.GetAllSSOUsers(ignoreFilters: true)
                .FirstOrDefault(s => s.Id == username || s.Email == username);

            ValidateAuth(user, password);

            return user;
        }

        public IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false)
            => ssoUserService.GetAllSSOUsers(ignoreFilters);

        public async ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser user)
        {
            var dbUser = GetAllSSOUsers().FirstOrDefault(u => u.Id == user.Id);
            var decryptedPassword = passwordCrypto.Decrypt(dbUser.PasswordHash);

            if (user.PasswordHash != decryptedPassword)
                user.PasswordHash = passwordCrypto.Encrypt(user.PasswordHash);

            return await ssoUserService.UpdateSSOUserAsync(user);
        }
    }
}