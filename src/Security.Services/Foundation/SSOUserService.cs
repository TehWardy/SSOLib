using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Foundation
{
    public class SSOUserService : ISSOUserService
    {
        readonly ISSOUserBroker ssoUserBroker;

        public SSOUserService(ISSOUserBroker storageBroker)
            => this.ssoUserBroker = storageBroker;

        public async ValueTask<SSOUser> AddSSOUserAsync(SSOUser item)
            => await ssoUserBroker.AddSSOUserAsync(item);

        public async ValueTask DeleteSSOUserAsync(SSOUser item)
            => await ssoUserBroker.DeleteSSOUserAsync(item);

        public async ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser item)
            => await ssoUserBroker.UpdateSSOUserAsync(item);

        public IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false)
            => ssoUserBroker.GetAllSSOUsers(ignoreFilters);
    }
}