using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VCommerce.Modules.Core.Infra;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            "Server=localhost;Port=5432;Database=vcommerce_db;Username=postgres;Password=qwe123;CommandTimeout=20");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}