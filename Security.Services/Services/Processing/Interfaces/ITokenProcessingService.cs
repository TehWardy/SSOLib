using Security.Objects.Entities;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ITokenProcessingService
    {
        ValueTask<Token> AddTokenForUser(string userId);
        ValueTask DeleteTokenAsync(string tokenId);
        Token GetTokenById(string id);
        ValueTask<Token> GenerateConfirmationToken(string userId, int reasonCode);
    }
}