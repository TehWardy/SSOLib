using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Data.Brokers.DateTime
{
    public class SecurityDateTimeOffsetBroker : ISecurityDateTimeOffsetBroker
    {
        public DateTimeOffset GetCurrentTime()
            => DateTimeOffset.Now;
    }
}
