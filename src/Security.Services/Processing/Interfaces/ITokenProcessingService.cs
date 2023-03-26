using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Services.Processing.Interfaces
{
    public interface ITokenProcessingService
    {
        ValueTask<Token> AddTokenForUserIdAsync(string userId);
        ValueTask DeleteTokenAsync(string tokenId);
        Token GetTokenById(string id);

        Token GetForgottenPasswordToken(string tokenId);
        Token GetConfirmationToken(string tokenId);
        ValueTask<Token> GenerateForgottenPasswordToken(string userId);
        ValueTask<Token> GenerateConfirmationToken(string userId);
    }
}