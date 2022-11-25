using System.Threading.Tasks;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;

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
