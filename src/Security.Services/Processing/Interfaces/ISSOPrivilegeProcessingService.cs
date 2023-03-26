using Security.Objects.Entities;
using System.Linq;

namespace Security.Services.Processing.Interfaces
{
    public interface ISSOPrivilegeProcessingService
    {
        public IQueryable<SSOPrivilege> GetAllSSOPrivileges();
    }
}