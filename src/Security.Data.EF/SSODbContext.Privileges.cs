using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Security.Objects.Entities;

namespace Security.Data.EF
{
    public partial class SSODbContext : DbContext
    {
        public IQueryable<SSOPrivilege> GetPrivileges()
        {
            // get managed set types 
            Type[] types = GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType)
                .Select(p => p.PropertyType.GenericTypeArguments[0])
                .ToArray();

            string suffix;

            // compute stnadardised priv set
            SSOPrivilege[] privs = types.SelectMany(t =>
                {
                    suffix = t.Name.EndsWith("s") ? "es" : "s";

                    // create CRUD privs for t
                    return new SSOPrivilege[]
                    {
                        new SSOPrivilege() { Id = $"{t.Name}_Create", Type = t.Name, Operation = "Create", Description = $"Allows users to Create {t.Name}{suffix}." },
                        new SSOPrivilege() { Id = $"{t.Name}_Read", Type = t.Name, Operation = "Read", Description = $"Allows users to Read {t.Name}{suffix}." },
                        new SSOPrivilege() { Id = $"{t.Name}_Update", Type = t.Name, Operation = "Update", Description = $"Allows users to Update {t.Name}{suffix}." },
                        new SSOPrivilege() { Id = $"{t.Name}_Delete", Type = t.Name, Operation = "Delete", Description = $"Allows users to Delete {t.Name}{suffix}." }
                    };
                })
                .ToArray();

            // lower the keys for consistency with other contexts
            foreach(var priv in privs)
                priv.Id = priv.Id.ToLower();

            return privs
                .GroupBy(p => p.Id).Select(g => g.First())
                .AsQueryable();
        }
    }
}