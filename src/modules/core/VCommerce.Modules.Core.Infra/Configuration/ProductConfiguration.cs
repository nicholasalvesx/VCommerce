using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.Description).HasColumnType("varchar(200)");
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Stock).IsRequired();
        builder.Property(x => x.CategoryName).IsRequired().HasColumnType("varchar(100)");
    }
}