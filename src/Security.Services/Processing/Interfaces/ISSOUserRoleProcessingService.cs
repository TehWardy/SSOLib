using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Services.Processing.Interfaces
{
    public interface ISSOUserRoleProcessingService
    {
        ValueTask<SSOUserRole> AddSSOUserRoleAsync(SSOUserRole item);
        ValueTask DeleteSSOUserRoleAsync(SSOUserRole item);
    }
}