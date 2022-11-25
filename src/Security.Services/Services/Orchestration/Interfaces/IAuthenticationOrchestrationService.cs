using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Services.Services.Orchestration.Interfaces
{
    public interface IAuthenticationOrchestrationService
    {
        ValueTask<Token> LoginAsync(string username, string password);
        ValueTask Logout(string tokenId = null);
        ValueTask<Token> GenerateForgotPasswordToken(string userId);
    }
}