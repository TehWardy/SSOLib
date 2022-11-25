using System.Threading.Tasks;
using Security.Objects.Entities;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface ISSORoleProcessingService
    {
        ValueTask<SSORole> AddSSORoleAsync(SSORole item);
        ValueTask<SSORole> UpdateSSORoleAsync(SSORole item);
        ValueTask DeleteSSORoleAsync(SSORole item);
    }
}