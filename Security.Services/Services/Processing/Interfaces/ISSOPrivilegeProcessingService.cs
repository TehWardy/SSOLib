using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ISSOPrivilegeProcessingService
    {
        public IQueryable<SSOPrivilege> GetAllSSOPrivileges();
    }
}