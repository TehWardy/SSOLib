using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Threading.Tasks;

namespace Security.Services.Processing
{
    public class SSOUserRoleProcessingService : ISSOUserRoleProcessingService
    {
        private readonly ISSOUserRoleService ssoUserRoleService;

        public SSOUserRoleProcessingService(ISSOUserRoleService ssoUserRoleService)
            => this.ssoUserRoleService = ssoUserRoleService;

        public async ValueTask<SSOUserRole> AddSSOUserRoleAsync(SSOUserRole item)
            => await ssoUserRoleService.AddSSOUserRoleAsync(item);

        public async ValueTask DeleteSSOUserRoleAsync(SSOUserRole item)
            => await ssoUserRoleService.DeleteSSOUserRoleAsync(item);
    }
}
