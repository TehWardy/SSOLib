using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.Services.Foundation
{
    public class SSORoleService : ISSORoleService
    {
        private readonly ISSORoleBroker roleBroker;

        public SSORoleService(ISSORoleBroker storageBroker)
            => this.roleBroker = storageBroker;

        public IQueryable<SSORole> GetAllSSORoles()
            => roleBroker.GetAllSSORoles();

        public async ValueTask<SSORole> AddSSORoleAsync(SSORole item)
            => await roleBroker.AddSSORoleAsync(item);

        public async ValueTask<SSORole> UpdateSSORoleAsync(SSORole item)
            => await roleBroker.UpdateSSORoleAsync(item);

        public async ValueTask DeleteSSORoleAsync(SSORole item)
            => await roleBroker.DeleteSSORoleAsync(item);
    }
}