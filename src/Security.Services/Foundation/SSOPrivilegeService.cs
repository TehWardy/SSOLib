﻿using Security.Data.Brokers.Storage.Interfaces;
using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using System.Linq;

namespace Security.Services.Foundation
{
    public class SSOPrivilegeService : ISSOPrivilegeService
    {
        readonly ISSOPrivilegeBroker privBroker;

        public SSOPrivilegeService(ISSOPrivilegeBroker privBroker)
            => this.privBroker = privBroker;

        public IQueryable<SSOPrivilege> GetAllSSOPrivileges()
            => privBroker.GetPrivileges();
    }
}