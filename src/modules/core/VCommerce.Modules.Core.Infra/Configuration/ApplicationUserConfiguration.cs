using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Configuration;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Id);
        builder.Property(u => u.Name).HasMaxLength(150);
        builder.Property(u => u.LastName).HasMaxLength(250);
        builder.Property(u => u.CustomerId).HasMaxLength(40);
        builder.Property(u => u.ConcurrencyStamp);
        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.Property(u => u.EmailConfirmed);
        builder.Property(u => u.LockoutEnabled);
        builder.Property(u => u.PasswordHash);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);
        builder.Property(u => u.SecurityStamp);
        builder.Property(u => u.AccessFailedCount);
        builder.Property(u => u.PhoneNumberConfirmed);
        builder.Property(u => u.TwoFactorEnabled);
        builder.Property(u => u.LockoutEnd);
        builder.Property(u => u.UpdatedAt);
        builder.Property(u => u.CreatedAt);
    }
}