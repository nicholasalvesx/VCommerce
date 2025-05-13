using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VCommerce.Modules.Core.Infra;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            "User Id=postgres.zhqwiofzfpgjbaxoyogt;Password=YW6F6kHabr7raGa7;Server=aws-0-sa-east-1.pooler.supabase.com;Port=5432;Database=postgres");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}