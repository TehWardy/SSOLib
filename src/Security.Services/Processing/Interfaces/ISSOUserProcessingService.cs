using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Processing.Interfaces
{
    public interface ISSOUserProcessingService
    {
        ValueTask<SSOUser> RegisterSSOUserAsync(SSOUser item);
        ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser item);
        ValueTask DeleteSSOUserAsync(SSOUser item);
        IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false);
        SSOUser FindByUserAndPassword(string username, string password);
        SSOUser FindById(string id);
        SSOUser Me();
    }
}