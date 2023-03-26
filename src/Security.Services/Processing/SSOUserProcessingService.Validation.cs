using Security.Objects.Entities;
using Security.Services.Processing.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
                .GetAllSSOUsers(true)
                .Any(sso => sso.Email == user.Email);

            if (emailInSystem)
                throw new ValidationException("Email exists");

            ValidatePassword(user.PasswordHash);
        }

        void ValidatePassword(string password)
        {
            if (password.Length < 8)
                throw new ValidationException("Password is too short");

            bool passwordHasLetters = password.Any(c => char.IsLetter(c));
            bool passwordHasDigits = password.Any(c => char.IsNumber(c));
            bool passwordHasUpperCase = password.Any(c => char.IsUpper(c));
            bool passwordHasLowerCase = password.Any(c => char.IsLower(c));

            if (!(passwordHasLetters && passwordHasDigits))
                throw new ValidationException("Password must contain both letter and numbers.");

            if (!(passwordHasUpperCase && passwordHasLowerCase))
                throw new ValidationException("Password must contain uppercase and lower case characters.");
        }

        static void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ValidationException("User cannot be empty!");
        }
    }
}