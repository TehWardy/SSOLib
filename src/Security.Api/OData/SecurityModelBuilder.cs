using Microsoft.OData.Edm;
using Security.Objects.Entities;
using System;

namespace Security.Api.OData
{
    public class SecurityModelBuilder : ODataModelBuilder
    {
        public override ODataModel Build() => new()
        {
            Context = "Security",
            Description = "Single Sign On Membership Endpoints for the platform.",
            EDMModel = BuildModel()
        };

        private IEdmModel BuildModel()
        {

            // Sets
            _ = AddSet<SSOUser, string>();
            _ = AddSet<Tenant, string>();
            _ = AddSet<TenantAnalysis, Guid>();
            _ = AddSet<SSORole, Guid>();
            _ = AddSet<UserEvent, Guid>();

            _ = AddJoinSet<SSOUserRole, object>(i => new { i.UserId, i.RoleId });
            //_ = AddJoinSet<TenantRole, object>(i => new { i.TenantId, i.RoleId });

            // Functions
            _ = Builder.EntityType<SSOUser>().Collection.Function("Me").ReturnsFromEntitySet<SSOUser>("User");
            _ = Builder.EntityType<SSOUser>().Collection.Action("RegisterInApp").ReturnsFromEntitySet<SSOUser>("AppInUser");

            return Builder.GetEdmModel();
        }
    }
}
