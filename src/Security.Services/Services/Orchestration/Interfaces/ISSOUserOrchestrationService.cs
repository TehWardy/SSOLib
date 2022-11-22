using Security.Objects.DTOs;
using Security.Objects.Entities;

namespace Security.Services.Services.Orchestration.Interfaces
{
    public interface ISSOUserOrchestrationService
    {
        SSOUser GetUserByTokenId(string tokenId);
        SSOUser GetSSOUserById(string id);
        SSOUser GetSSOUserFromSession();

        ValueTask<SSOUser> Register(RegisterUser registerForm);
        ValueTask ChangePassword(string oldPassword, string newPassword);
        ValueTask ConfirmForgotPassword(string tokenId, string newPassword);
        ValueTask ConfirmRegistration(string tokenId);
    }
}