using Security.Objects.Entities;
using System.Linq;

namespace Security.Services.Foundation.Interfaces
{
    public interface ISSOPrivilegeService
    {
        IQueryable<SSOPrivilege> GetAllSSOPrivileges();
    }
}