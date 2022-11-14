using System;

namespace Security.Objects.Entities
{
    public class UserActivity
    {
        // tenant details
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantDescription { get; set; }
        public string TenantCreatedBy { get; set; }
        public string TenantLastUpdatedBy { get; set; }
        public DateTimeOffset TenantCreatedOn { get; set; }
        public DateTimeOffset TenantLastUpdated { get; set; }

        // user details
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }

        // event details
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string EventValue { get; set; }
        public DateTimeOffset EventCreatedOn { get; set; }

        // session details
        public string SessionId { get; set; }
        public DateTimeOffset SessionExpiresAtTime { get; set; }
    }
}