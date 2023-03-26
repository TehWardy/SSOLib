using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Foundation.Interfaces
{
    public interface ISSOUserRoleService
    {
        IQueryable<SSOUserRole> GetAllSSOUserRoles();

        ValueTask<SSOUserRole> AddSSOUserRoleAsync(SSOUserRole item);
        ValueTask DeleteSSOUserRoleAsync(SSOUserRole item);
    }
}