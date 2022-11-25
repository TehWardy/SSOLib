using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ISSOUserRoleProcessingService
    {
        ValueTask<SSOUserRole> AddSSOUserRoleAsync(SSOUserRole item);
        ValueTask DeleteSSOUserRoleAsync(SSOUserRole item);
    }
}