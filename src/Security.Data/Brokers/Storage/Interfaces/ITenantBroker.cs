using System.Linq;
using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ITenantBroker
    {
        ValueTask<Tenant> AddTenantAsync(Tenant tenant);
        ValueTask DeleteTenantAsync(Tenant tenant);
        IQueryable<Tenant> GetAllTenants();
        ValueTask<Tenant> UpdateTenantAsync(Tenant tenant);
    }
}