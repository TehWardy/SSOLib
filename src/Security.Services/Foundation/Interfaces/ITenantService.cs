using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Foundation.Interfaces
{
    public interface ITenantService
    {
        ValueTask<Tenant> AddTenantAsync(Tenant tenant);
        ValueTask DeleteTenantAsync(Tenant tenant);
        IQueryable<Tenant> GetAllTenants();
        ValueTask<Tenant> UpdateTenantAsync(Tenant tenant);
    }
}
