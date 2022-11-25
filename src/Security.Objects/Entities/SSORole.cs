using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Objects.Entities
{
    public class SSORole
    {
        public Guid Id { get; set; }

        public bool UsersArePortalAdmins { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Privs { get; set; }

        public string TenantId { get; set; }

        public Tenant Tenant { get; set; }

        public virtual ICollection<SSOUserRole> Users { get; set; }

    }
}