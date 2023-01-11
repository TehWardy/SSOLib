using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Security.Objects.Entities;
using Security.Services.Services.Processing.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace B2B.Api.Controllers.Transactions
{
    public class TenantController : Controller
    {
        readonly ITenantProcessingService tenantProcessingService;

        public TenantController(ITenantProcessingService tenantProcessingService)
            => this.tenantProcessingService = tenantProcessingService;

        [HttpGet]
        [EnableQuery]
        public virtual IActionResult Get(ODataQueryOptions<Tenant> queryOptions)
            => Ok(tenantProcessingService.GetAllTenants());

        [HttpGet]
        [EnableQuery]
        public virtual IActionResult Get([FromRoute] string key)
        {
            IQueryable<Tenant> result = tenantProcessingService
                .GetAllTenants()
                .AsQueryable()
                .Where(i => i.Id == key);

            return result.Any()
                ? Ok(SingleResult.Create(result))
                : NotFound();
        }

        [HttpPost]
        [EnableQuery]
        public virtual async ValueTask<IActionResult> Post([FromBody] Tenant tenant)
            => ModelState.IsValid
                ? Ok(await tenantProcessingService.AddTenantAsync(tenant))
                : BadRequest(ModelState);

        [HttpPut]
        [EnableQuery]
        public virtual async ValueTask<IActionResult> Put([FromRoute] string key, [FromBody] Tenant tenant)
            => ModelState.IsValid
                ? Ok(await tenantProcessingService.UpdateTenantAsync(tenant))
                : BadRequest(ModelState);


        [HttpDelete]
        public virtual async ValueTask<IActionResult> Delete([FromRoute] string key)
        {
            Tenant origentity = tenantProcessingService.GetAllTenants().FirstOrDefault(i => i.Id == key);

            if (origentity == null)
                return NotFound();

            await tenantProcessingService.DeleteTenantAsync(origentity);
            return Ok();
        }
    }
}