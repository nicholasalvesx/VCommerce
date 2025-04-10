using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Data;
using VCommerce.Api.Models;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
    {
        opt.Lockout.MaxFailedAccessAttempts = 50;
        opt.SignIn.RequireConfirmedPhoneNumber = false;
        opt.SignIn.RequireConfirmedEmail = true;
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireDigit = false;
        opt.Password.RequiredLength = 4;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<Worker>();

var host = builder.Build();

try
{
    using var scope = host.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Starting database migrations and seeds...");
    
    var identityMigrator = scope.ServiceProvider.GetRequiredService<Worker>();
    await identityMigrator.StartAsync(CancellationToken.None);
    logger.LogInformation("Identity migrations and seeds completed");
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error during application initialization. Details: {Message}", 
        ex.Message);
            
    if (ex.InnerException != null)
    {
        logger.LogError("Inner exception: {Message}", ex.InnerException.Message);
    }
            
    throw;
}

await host.RunAsync();