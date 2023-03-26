using Security.Objects.Entities;
using System.Threading.Tasks;

namespace Security.Services.Processing.Interfaces
{
    public interface ISSORoleProcessingService
    {
        ValueTask<SSORole> AddSSORoleAsync(SSORole item);
        ValueTask<SSORole> UpdateSSORoleAsync(SSORole item);
        ValueTask DeleteSSORoleAsync(SSORole item);
    }
}