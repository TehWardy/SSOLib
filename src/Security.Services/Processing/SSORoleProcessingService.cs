﻿using Security.Objects.Entities;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing.Interfaces;
using System.Threading.Tasks;

namespace Security.Services.Processing
{
    public class SSORoleProcessingService : ISSORoleProcessingService
    {
        private readonly ISSORoleService ssoRoleService;

        public SSORoleProcessingService(ISSORoleService ssoRoleService)
            => this.ssoRoleService = ssoRoleService;

        public async ValueTask<SSORole> AddSSORoleAsync(SSORole item)
            => await ssoRoleService.AddSSORoleAsync(item);

        public async ValueTask DeleteSSORoleAsync(SSORole item)
            => await ssoRoleService.DeleteSSORoleAsync(item);

        public async ValueTask<SSORole> UpdateSSORoleAsync(SSORole item)
            => await ssoRoleService.UpdateSSORoleAsync(item);
    }
}