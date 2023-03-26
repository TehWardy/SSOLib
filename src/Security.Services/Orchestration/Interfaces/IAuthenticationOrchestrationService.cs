using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Services.Orchestration.Interfaces
{
    public interface IAuthenticationOrchestrationService
    {
        ValueTask<Token> LoginAsync(string username, string password);
        ValueTask Logout(string tokenId = null);
        ValueTask<Token> GenerateForgotPasswordToken(string userId);
    }
}