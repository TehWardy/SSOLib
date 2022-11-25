using System.Linq;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ISSOPrivilegeBroker
    {
        IQueryable<SSOPrivilege> GetPrivileges();
    }
}