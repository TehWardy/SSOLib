using Security.Objects.Entities;
using Security.Services.Services.Processing.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Security.Services.Processing
{
    public partial class SSOUserProcessingService : ISSOUserProcessingService
    {
        public void ValidateSSOUser(SSOUser user)
        {
            if (!user.Email.Contains('@'))
                throw new ValidationException("Invalid email provided");

            if (string.IsNullOrEmpty(user.DisplayName))
                throw new ValidationException("Display name cannot be empty");

            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new ValidationException("Password cannot be empty");

            var emailInSystem = ssoUserService
                .GetAllSSOUsers()
                .Any(sso => sso.Email == user.Email);

            if (emailInSystem)
                throw new ValidationException("Email exists");

            ValidatePassword(user.PasswordHash);
        }

        void ValidatePassword(string password)
        {
            if (password.Length < 8)
                throw new ValidationException("Password is too short");

            if (password.Any(c => char.IsLetter(c) && password.Any(c => !char.IsLetter(c))))
                throw new ValidationException("Password must contain both letter and non letter characters.");

            if (password.Any(c => char.IsLower(c) && password.Any(c => !char.IsLower(c))))
                throw new ValidationException("Password must contain uppercase and lower case characters.");
        }

        static void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ValidationException("User cannot be empty!");
        }

        void ValidateAuth(SSOUser user, string password)
        {
            var decryptedPassword = passwordCrypto.Decrypt(user.PasswordHash);

            bool allGood =
                string.IsNullOrEmpty(password) &&
                user == null &&
                password != decryptedPassword &&
                user == null;

            if (!allGood)
                throw new SecurityException("Access Denied!");
        }
    }
}