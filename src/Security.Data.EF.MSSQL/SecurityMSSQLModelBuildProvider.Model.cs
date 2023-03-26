using Microsoft.EntityFrameworkCore;
using Security.Objects.Entities;

namespace Security.Data.EF.MSSQL;

public partial class SecurityMSSQLModelBuildProvider
{
    public static void ConfigureSecurityModel(ModelBuilder modelBuilder)
    {
        ConfigureSecurityTables(modelBuilder);
        ConfigureSecurityColumns(modelBuilder);
        ConfigureSecurityForeignKeys(modelBuilder);
    }

    public static void ConfigureSecurityTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SSORole>()
            .ToTable("Roles");

        modelBuilder.Entity<SSOUser>()
            .ToTable("Users");

        modelBuilder.Entity<SSOUserRole>()
            .ToTable("UserRoles");

        modelBuilder.Entity<Tenant>()
            .ToTable("Tenants");

        modelBuilder.Entity<TenantAnalysis>()
            .ToTable("TenantAnalysis");

        modelBuilder.Entity<Token>()
            .ToTable("Tokens");

        modelBuilder.Entity<UserEvent>()
            .ToTable("UserEvents");
    }

    public static void ConfigureSecurityColumns(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SSORole>()
            .HasKey(ssoRole => ssoRole.Id);

        modelBuilder.Entity<SSORole>()
            .Property(ssoRole => ssoRole.Name)
            .IsRequired();

        modelBuilder.Entity<SSOUser>()
            .HasKey(ssoUser => ssoUser.Id);

        modelBuilder.Entity<SSOUserRole>()
            .HasKey(ssoUserRole => new { ssoUserRole.RoleId, ssoUserRole.UserId });

        modelBuilder.Entity<Tenant>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Tenant>()
            .Property(t => t.Id)
            .HasMaxLength(50);

        modelBuilder.Entity<Tenant>()
            .Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Tenant>()
            .Property(t => t.Description)
            .HasMaxLength(500)
            .IsRequired();

        modelBuilder.Entity<Tenant>()
            .Property(t => t.CreatedBy)
            .HasMaxLength(100);

        modelBuilder.Entity<Tenant>()
            .Property(t => t.LastUpdatedBy)
            .HasMaxLength(100);

        modelBuilder.Entity<TenantAnalysis>()
            .HasKey(ta => ta.Id);

        modelBuilder.Entity<Token>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Token>()
            .Property(t => t.Id)
            .HasMaxLength(64);

        modelBuilder.Entity<UserEvent>()
            .Property(ue => ue.Id);
    }

    public static void ConfigureSecurityForeignKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SSORole>()
            .HasMany(ssoRole => ssoRole.Users)
            .WithOne(ssoUserRole => ssoUserRole.Role)
            .HasForeignKey(ssoUserRole => ssoUserRole.RoleId);

        modelBuilder.Entity<SSORole>()
            .HasOne(ssoRole => ssoRole.Tenant)
            .WithMany(t => t.Roles)
            .HasForeignKey(ssoRole => ssoRole.TenantId);

        modelBuilder.Entity<SSOUser>()
            .HasMany(ssoUser => ssoUser.Roles)
            .WithOne(ssoUserRole => ssoUserRole.User)
            .HasForeignKey(ssoUserRole => ssoUserRole.UserId);

        modelBuilder.Entity<SSOUser>()
            .HasMany(ssoUser => ssoUser.UserEvents)
            .WithOne(userEvent => userEvent.CreatedByUser)
            .HasForeignKey(userEvent => userEvent.CreatedBy);

        modelBuilder.Entity<SSOUser>()
            .HasMany(ssoUser => ssoUser.Tokens)
            .WithOne(token => token.User)
            .HasForeignKey(token => token.UserName);

        modelBuilder.Entity<SSOUserRole>()
            .HasOne(ssoUserRole => ssoUserRole.Role)
            .WithMany(ssoRole => ssoRole.Users)
            .HasForeignKey(ssoUserRole => ssoUserRole.RoleId);

        modelBuilder.Entity<SSOUserRole>()
            .HasOne(ssoUserRole => ssoUserRole.User)
            .WithMany(ssoUser => ssoUser.Roles)
            .HasForeignKey(ssoUserRole => ssoUserRole.UserId);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Roles)
            .WithOne(ssoRole => ssoRole.Tenant)
            .HasForeignKey(ssoRole => ssoRole.TenantId);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.UserEvents)
            .WithOne(userEvent => userEvent.Tenant)
            .HasForeignKey(userEvent => userEvent.TenantId);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Analysis)
            .WithOne(analysis => analysis.Tenant)
            .HasForeignKey(analysis => analysis.TenantId);

        modelBuilder.Entity<TenantAnalysis>()
            .HasOne(ta => ta.Tenant)
            .WithMany(t => t.Analysis)
            .HasForeignKey(ta => ta.TenantId);

        modelBuilder.Entity<Token>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(t => t.UserName);

        modelBuilder.Entity<UserEvent>()
            .HasOne(ur => ur.Session)
            .WithMany(s => s.UserEvents)
            .HasForeignKey(ur => ur.SessionId);

        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.Tenant)
            .WithMany(t => t.UserEvents)
            .HasForeignKey(ue => ue.TenantId);

        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.CreatedByUser)
            .WithMany(ssoUser => ssoUser.UserEvents)
            .HasForeignKey(ue => ue.CreatedBy);
    }
}

