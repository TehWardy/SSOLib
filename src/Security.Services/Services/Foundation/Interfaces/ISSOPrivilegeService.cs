using System.Linq;
using Security.Objects.Entities;

namespace Security.Services.Foundation.Interfaces
{
    public interface ISSOPrivilegeService
    {
        IQueryable<SSOPrivilege> GetAllSSOPrivileges();
    }
}