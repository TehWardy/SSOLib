using System.Linq;
using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ITenantAnalysisBroker
    {
        ValueTask<TenantAnalysis> AddTenantAnalysisAsync(TenantAnalysis tenantAnalysis);
        ValueTask DeleteTenantAnalysisAsync(TenantAnalysis tenantAnalysis);
        IQueryable<TenantAnalysis> GetAllTenantAnalysiss();
        ValueTask<TenantAnalysis> UpdateTenantAnalysisAsync(TenantAnalysis tenantAnalysis);
    }
}