using Security.Objects.Entities;

namespace Security.Data.Brokers.Authentication
{
    public interface IIdentityBroker
    {
        SSOUser Me();
    }
}