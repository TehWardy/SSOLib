using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Processing.Interfaces
{
    public interface ITenantProcessingService
    {
        ValueTask<Tenant> AddTenantAsync(Tenant item);
        ValueTask<Tenant> UpdateTenantAsync(Tenant item);
        ValueTask DeleteTenantAsync(Tenant item);
        IQueryable<Tenant> GetAllTenants();
    }
}
