using Security.Data.Brokers.Encryption;
using Security.Data.Brokers.Storage;
using Security.Data.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;

namespace Security.Services.Processing
{
    public partial class SSOUserProcessingService : ISSOUserProcessingService
    {
        readonly ISSOUserService ssoUserService;
        private readonly IPasswordEncryptionBroker encryptionBroker;

        public SSOUserProcessingService(ISSOUserService ssoUserService, IPasswordEncryptionBroker encryptionBroker)
        {         
            this.ssoUserService = ssoUserService;
            this.encryptionBroker = encryptionBroker;
        }

        public async ValueTask<SSOUser> RegisterSSOUserAsync(SSOUser user)
        {
            ValidateSSOUser(user);
            var userIdCount = ssoUserService.GetAllSSOUsers(true)
                .Count(sso => sso.Id == user.Id);

            if (userIdCount > 0)
                user.Id += userIdCount;

            user.PasswordHash = encryptionBroker.Encrypt(user.PasswordHash);

            return await ssoUserService.AddSSOUserAsync(user);
        }

        public async ValueTask DeleteSSOUserAsync(SSOUser item)
            => await ssoUserService.DeleteSSOUserAsync(item);

        public SSOUser FindByUserAndPassword(string username, string password)
        {
            ValidateUsername(username);

            var user = ssoUserService.GetAllSSOUsers(ignoreFilters: true)
                .FirstOrDefault(s => s.Id == username || s.Email == username);

            if (user == null)
                throw new SecurityException("Access Denied!");

            if (!encryptionBroker.EncryptedAndPlainTextAreEqual(user.PasswordHash, password))
                throw new SecurityException("Access Denied!");

            return user;
        }

        public SSOUser FindByTokenId(string tokenId)
            => ssoUserService.GetAllSSOUsers()
                .FirstOrDefault(u => u.Tokens.Any(t => t.Id == tokenId));

        public IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false)
            => ssoUserService.GetAllSSOUsers(ignoreFilters);

        public async ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser user)
        {
            var dbUser = GetAllSSOUsers()
                .FirstOrDefault(u => u.Id == user.Id);

            if (dbUser.PasswordHash != user.PasswordHash && !encryptionBroker.EncryptedAndPlainTextAreEqual(dbUser.PasswordHash, user.PasswordHash))
                user.PasswordHash = encryptionBroker.Encrypt(user.PasswordHash);

            return await ssoUserService.UpdateSSOUserAsync(user);
        }


    }
}