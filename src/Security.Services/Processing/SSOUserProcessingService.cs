﻿using Security.Data.Brokers.Authentication;
using Security.Data.Brokers.Encryption;
using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Security.Services.Processing
{
    public partial class SSOUserProcessingService : ISSOUserProcessingService
    {
        private readonly ISSOUserService ssoUserService;
        private readonly IPasswordEncryptionBroker encryptionBroker;
        private readonly IIdentityBroker identityBroker;

        public SSOUserProcessingService(ISSOUserService ssoUserService,
            IPasswordEncryptionBroker encryptionBroker,
            IIdentityBroker identityBroker)
        {
            this.ssoUserService = ssoUserService;
            this.encryptionBroker = encryptionBroker;
            this.identityBroker = identityBroker;
        }

        public async ValueTask<SSOUser> RegisterSSOUserAsync(SSOUser user)
        {
            ValidateSSOUser(user);

            var userIdCount = ssoUserService.GetAllSSOUsers(ignoreFilters: true)
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

            var user = FindById(username);

            if (user == null)
                throw new SecurityException("Access Denied!");

            if (user.LockoutEnabled)
                throw new SecurityException("Access Denied!");

            if (!encryptionBroker.EncryptedAndPlainTextAreEqual(user.PasswordHash, password))
                throw new SecurityException("Access Denied!");

            return user;
        }

        public SSOUser FindById(string id)
            => ssoUserService.GetAllSSOUsers(ignoreFilters: true)
                .FirstOrDefault(u => u.Id == id || u.Email == id);

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

        public SSOUser Me()
            => identityBroker.Me();
    }
}