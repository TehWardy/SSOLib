using Microsoft.Extensions.Configuration;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.Services.Foundation
{
    public class TokenService : ITokenService
    {
        readonly ITokenBroker storageBroker;
        readonly int tokenTimeout = 45;

        public TokenService(ITokenBroker storageBroker, IConfiguration cofiguration)
        {
            this.storageBroker = storageBroker;

            if (int.TryParse(cofiguration?.GetSection("Settings")["TokenTimeout"], out int timeout))
                tokenTimeout = timeout;
        }

        public async ValueTask<Token> AddTokenAsync(string userId, int reasonCode = 0)
        {
            string value = Guid.NewGuid().ToString().Replace("-", "") + Guid.NewGuid().ToString().Replace("-", "");

            if (value.StartsWith("a"))
                value = value[1..] + "a";

            return await storageBroker.AddTokenAsync(new Token()
            {
                Id = value,
                Expires = DateTimeOffset.Now.AddMinutes(tokenTimeout),
                Reason = reasonCode,
                UserName = userId
            });
        }

        public async ValueTask DeleteTokenAsync(Token item)
            => await storageBroker.DeleteTokenAsync(item);

        public IQueryable<Token> GetAllTokens(bool ignoreFilters = false)
            => storageBroker.GetAllTokens(ignoreFilters);
    }
}