using System;

namespace Security.Data.Brokers.DateTime
{
    public interface ISecurityDateTimeOffsetBroker
    {
        DateTimeOffset GetCurrentTime();
    }
}