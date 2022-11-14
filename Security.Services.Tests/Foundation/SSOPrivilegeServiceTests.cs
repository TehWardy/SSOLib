using FizzWare.NBuilder;
using Moq;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation;
using Security.Services.Foundation.Interfaces;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOPrivilegeServiceTests
    {
        readonly Mock<ISSOPrivilegeBroker> privBrokerMock;
        readonly ISSOPrivilegeService privService;

        public SSOPrivilegeServiceTests()
        {
            privBrokerMock = new Mock<ISSOPrivilegeBroker>();
            privService = new SSOPrivilegeService(privBrokerMock.Object);
        }

        static IQueryable<SSOPrivilege> RandomSSOPrivileges()
            => Enumerable.Range(0, new Random().Next(100))
                .Select(i => RandomSSOPrivilege())
                .AsQueryable();

        static SSOPrivilege RandomSSOPrivilege()
            => Builder<SSOPrivilege>
                .CreateNew()
                .Build();
    }
}