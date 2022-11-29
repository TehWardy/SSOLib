using System;

namespace Security.Objects.Entities
{
    public class UserEvent
    {
        public Guid Id { get; set; }

        public string EventName { get; set; }

        public string Value { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string SessionId { get; set; }

        public virtual Session Session { get; set; }

        public string TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

        public string CreatedBy { get; set; }

        public virtual SSOUser CreatedByUser { get; set; }
    }
}