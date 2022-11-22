using Security.Objects.Entities;

namespace Security.Services.Services.Orchestration.Interfaces
{
    public interface IAuthenticationOrchestrationService
    {
        ValueTask<Token> GenerateConfirmationToken(string userId, int reasonCode);
        ValueTask<Token> LoginAsync(string username, string password);
        ValueTask Logout(string tokenId = null);
    }
}