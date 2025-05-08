using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.Email).HasColumnName("Email").IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.Password).HasColumnName("Password").IsRequired().HasColumnType("varchar(30)");
        builder.Property(x => x.LastName).HasColumnName("LastName").IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.ConfirmPassword).HasColumnName("ConfirmPassword").IsRequired().HasColumnType("varchar(100)");
    }
}