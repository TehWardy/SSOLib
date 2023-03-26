using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Processing
{
    public class TenantProcessingService : ITenantProcessingService
    {
        private readonly ITenantService tenantService;

        public TenantProcessingService(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        public ValueTask<Tenant> AddTenantAsync(Tenant item)
            => tenantService.AddTenantAsync(item);

        public ValueTask DeleteTenantAsync(Tenant item)
            => tenantService.DeleteTenantAsync(item);

        public IQueryable<Tenant> GetAllTenants()
            => tenantService.GetAllTenants();

        public ValueTask<Tenant> UpdateTenantAsync(Tenant item)
            => tenantService.UpdateTenantAsync(item);
    }
}
