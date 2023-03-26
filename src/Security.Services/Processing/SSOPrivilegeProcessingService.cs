using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Linq;

namespace Security.Services.Processing
{
    public class SSOPrivilegeProcessingService : ISSOPrivilegeProcessingService
    {
        readonly ISSOPrivilegeService privService;

        public SSOPrivilegeProcessingService(ISSOPrivilegeService privService)
            => this.privService = privService;

        public IQueryable<SSOPrivilege> GetAllSSOPrivileges()
            => privService.GetAllSSOPrivileges();
    }
}
