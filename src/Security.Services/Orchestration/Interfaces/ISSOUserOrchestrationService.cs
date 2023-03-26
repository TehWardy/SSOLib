using Security.Objects.DTOs;
using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Services.Orchestration.Interfaces
{
    public interface ISSOUserOrchestrationService
    {
        ValueTask<SSOUser> Register(RegisterUser registerForm);
        ValueTask ConfirmForgotPassword(string tokenId, string newPassword);
        ValueTask ConfirmRegistration(string tokenId);
    }
}