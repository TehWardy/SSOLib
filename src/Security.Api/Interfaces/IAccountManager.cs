using Security.Objects.DTOs;
using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Api.Interfaces
{
    public interface IAccountManager
    {
        SSOUser Me();
        ValueTask<Token> LoginAsync(string username, string password);
        ValueTask LogoutAsync();
        ValueTask<SSOUser> RegisterAsync(RegisterUser registerForm);
        ValueTask ChangePasswordAsync(string oldPassword, string newPassword);
        ValueTask ForgotPasswordAsync(string email, int appId);
        ValueTask ConfirmRegistrationAsync(string token);
        ValueTask ConfirmForgotPasswordAsync(string token, string newPassword);
    }
}
