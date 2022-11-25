using System.Linq;
using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage.Interfaces
{
    public interface ISSORoleBroker
    {
        ValueTask<SSORole> AddSSORoleAsync(SSORole SSORole);
        ValueTask DeleteSSORoleAsync(SSORole SSORole);
        IQueryable<SSORole> GetAllSSORoles();
        ValueTask<SSORole> UpdateSSORoleAsync(SSORole SSORole);
    }
}