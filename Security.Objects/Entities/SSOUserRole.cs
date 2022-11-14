using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Objects.Entities
{
    public class SSOUserRole
    {
        public Guid RoleId { get; set; }

        public virtual SSORole Role { get; set; }

        public string UserId { get; set; }

        public virtual SSOUser User { get; set; }

    }
}