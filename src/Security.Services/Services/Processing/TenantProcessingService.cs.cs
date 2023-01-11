using Security.Data.Brokers.Authentication;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Services.Processing
{
    public class TenantProcessingService : ITenantProcessingService
    {
        private readonly ITenantService tenantService;
        private readonly IIdentityBroker identityBroker;

        public TenantProcessingService(ITenantService tenantService, IIdentityBroker identityBroker)
        {
            this.tenantService = tenantService;
            this.identityBroker = identityBroker;
        }

        public ValueTask<Tenant> AddTenantAsync(Tenant item)
        {
            var user = identityBroker.Me();

            item.CreatedBy = user.Id;
            item.LastUpdatedBy= user.Id;

            return tenantService.AddTenantAsync(item);
        }
           

        public ValueTask DeleteTenantAsync(Tenant item)
            => tenantService.DeleteTenantAsync(item);

        public IQueryable<Tenant> GetAllTenants()
            => tenantService.GetAllTenants();

        public ValueTask<Tenant> UpdateTenantAsync(Tenant item)
            => tenantService.UpdateTenantAsync(item);
    }
}
