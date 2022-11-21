using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Objects.Entities
{
    public class Session
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ExpiresAtTime { get; set; }
        public long SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset AbsoluteExpiration { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}