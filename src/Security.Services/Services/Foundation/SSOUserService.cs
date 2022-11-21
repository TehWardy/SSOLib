using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.Services.Foundation
{
    public class SSOUserService : ISSOUserService
    {
        readonly ISSOUserBroker userBrokerMock;
        
        public SSOUserService(ISSOUserBroker storageBroker)
            => this.userBrokerMock = storageBroker;

        public async ValueTask<SSOUser> AddSSOUserAsync(SSOUser item)
        {
            var userIdCount = userBrokerMock.GetAllSSOUsers().Count(sso => sso.Id == item.Id);
            if (userIdCount > 0)
                item.Id = item.Id + userIdCount;

            return await userBrokerMock.AddSSOUserAsync(item);
        }

        public async ValueTask DeleteSSOUserAsync(SSOUser item)
            => await userBrokerMock.DeleteSSOUserAsync(item);

        public async ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser item)
            => await userBrokerMock.UpdateSSOUserAsync(item);

        public IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false)
            => userBrokerMock.GetAllSSOUsers(ignoreFilters);
    }
}