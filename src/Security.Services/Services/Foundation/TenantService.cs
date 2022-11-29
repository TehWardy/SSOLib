using Security.Data.Brokers.DateTime;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Services.Foundation
{
    public class TenantService : ITenantService
    {
        private readonly ITenantBroker broker;
        private readonly ISecurityDateTimeOffsetBroker dateTimeOffsetBroker;

        public TenantService(ITenantBroker broker, ISecurityDateTimeOffsetBroker dateTimeOffsetBroker)
        {
            this.broker = broker;
            this.dateTimeOffsetBroker = dateTimeOffsetBroker;
        }

        public async ValueTask<Tenant> AddTenantAsync(Tenant tenant)
        {
            tenant.LastUpdated = dateTimeOffsetBroker.GetCurrentTime();
            tenant.CreatedOn = dateTimeOffsetBroker.GetCurrentTime();
            return await broker.AddTenantAsync(tenant);
        }

        public async ValueTask DeleteTenantAsync(Tenant tenant)
            => await broker.DeleteTenantAsync(tenant);

        public IQueryable<Tenant> GetAllTenants()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Tenant> UpdateTenantAsync(Tenant tenant)
        {
            tenant.LastUpdated = dateTimeOffsetBroker.GetCurrentTime();
            return await broker.UpdateTenantAsync(tenant);
        }
    }
}
