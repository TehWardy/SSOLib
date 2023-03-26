using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Foundation.Interfaces
{
    public interface ISSOUserService
    {
        ValueTask<SSOUser> AddSSOUserAsync(SSOUser item);
        ValueTask<SSOUser> UpdateSSOUserAsync(SSOUser item);
        ValueTask DeleteSSOUserAsync(SSOUser item);
        IQueryable<SSOUser> GetAllSSOUsers(bool ignoreFilters = false);
    }
}