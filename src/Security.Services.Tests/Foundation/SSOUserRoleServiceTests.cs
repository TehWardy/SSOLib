using System;
using System.Linq;
using FizzWare.NBuilder;
using Moq;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation;
using Security.Services.Services.Foundation.Interfaces;

namespace Security.Services.Tests.Foundation
{
    public partial class SSOUserRoleServiceTests
    {
        readonly Mock<ISSOUserRoleBroker> userRoleBrokerMock;
        readonly ISSOUserRoleService userRoleService;

        public SSOUserRoleServiceTests()
        {
            userRoleBrokerMock = new Mock<ISSOUserRoleBroker>();
            userRoleService = new SSOUserRoleService(userRoleBrokerMock.Object);
        }

        static IQueryable<SSOUserRole> RandomUserRoles()
            => Enumerable.Range(0, new Random().Next(100))
                .Select(i => RandomUserRole())
                .AsQueryable();

        static SSOUserRole RandomUserRole()
            => Builder<SSOUserRole>
                .CreateNew()
                .Build();
    }
}