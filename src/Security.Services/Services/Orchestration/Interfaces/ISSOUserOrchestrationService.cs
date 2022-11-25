using System.Threading.Tasks;
using Security.Objects.DTOs;
using Security.Objects.Entities;

namespace Security.Services.Services.Orchestration.Interfaces
{
    public interface ISSOUserOrchestrationService
    {
        ValueTask<SSOUser> Register(RegisterUser registerForm);
        ValueTask ConfirmForgotPassword(string tokenId, string newPassword);
        ValueTask ConfirmRegistration(string tokenId);
    }
}