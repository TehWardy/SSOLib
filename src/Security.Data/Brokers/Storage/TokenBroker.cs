﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage
{
    public class TokenBroker : ITokenBroker
    {
        ISSODbContextFactory contextFactory;

        public TokenBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public async ValueTask<Token> AddTokenAsync(Token token)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Tokens.Add(token);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<Token> UpdateTokenAsync(Token token)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Tokens.Update(token);
            await context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask DeleteTokenAsync(Token token)
        {
            using var context = contextFactory.CreateDbContext();

            var entityEntry = context.Tokens.Remove(token);
            await context.SaveChangesAsync();
        }

        public IQueryable<Token> GetAllTokens(bool ignoreFilters = false)
        {
            var context = contextFactory.CreateDbContext();

            return ignoreFilters
                ? context.Tokens.IgnoreQueryFilters()
                : context.Tokens;
        }
    }
}