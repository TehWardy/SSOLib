using System.Linq;
using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Services.Services.Foundation.Interfaces
{
    public interface ITokenService
    {
        ValueTask<Token> AddTokenAsync(string userId, int reasonCode = 0);
        ValueTask DeleteTokenAsync(Token item);
        IQueryable<Token> GetAllTokens(bool ignoreFilters = false);
    }
}