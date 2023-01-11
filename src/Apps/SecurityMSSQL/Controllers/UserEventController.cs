using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Security.Objects.Entities;
using Security.Services.Services.Processing.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace B2B.Api.Controllers.Transactions
{
    public class UserEventController : Controller
    {
        readonly IUserEventProcessingService userEventProcessingService;

        public UserEventController(IUserEventProcessingService userEventProcessingService)
            => this.userEventProcessingService = userEventProcessingService;

        [HttpGet]
        [EnableQuery]
        public virtual IActionResult Get(ODataQueryOptions<UserEvent> queryOptions)
            => Ok(userEventProcessingService.GetAllUserEvents());

        [HttpGet]
        [EnableQuery]
        public virtual IActionResult Get([FromRoute] Guid key)
        {
            IQueryable<UserEvent> result = userEventProcessingService
                .GetAllUserEvents()
                .AsQueryable()
                .Where(i => i.Id == key);

            return result.Any()
                ? Ok(SingleResult.Create(result))
                : NotFound();
        }

        [HttpPost]
        [EnableQuery]
        public virtual async ValueTask<IActionResult> Post([FromBody] UserEvent userEvent)
            => ModelState.IsValid
                ? Ok(await userEventProcessingService.AddUserEventAsync(userEvent))
                : BadRequest(ModelState);

        [HttpDelete]
        public virtual async ValueTask<IActionResult> Delete([FromRoute] Guid key)
        {
            UserEvent origentity = userEventProcessingService.GetAllUserEvents().FirstOrDefault(i => i.Id == key);

            if (origentity == null)
                return NotFound();

            await userEventProcessingService.DeleteUserEventAsync(origentity);
            return Ok();
        }
    }
}