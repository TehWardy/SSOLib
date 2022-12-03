using Security.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ITenantProcessingService
    {
        ValueTask<Tenant> AddTenantAsync(Tenant item);
        ValueTask<Tenant> UpdateTenantAsync(Tenant item);
        ValueTask DeleteTenantAsync(Tenant item);
        IQueryable<Tenant> GetAllTenants();
    }
}
