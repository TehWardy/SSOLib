using Security.Objects.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services.Services.Processing.Interfaces
{
    public interface IUserEventProcessingService
    {
        ValueTask<UserEvent> AddUserEventAsync(UserEvent userEvent);
        ValueTask DeleteUserEventAsync(UserEvent userEvent);
        IQueryable<UserEvent> GetAllUserEvents();
    }
}
