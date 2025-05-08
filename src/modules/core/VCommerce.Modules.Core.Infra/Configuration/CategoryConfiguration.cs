using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");
        
        builder.HasKey(x => x.CategoryId);
        
        builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
    }
}