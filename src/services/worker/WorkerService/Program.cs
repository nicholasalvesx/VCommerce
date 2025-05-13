using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VCommerce.Modules.Core.Infra;
using VCommerce.Modules.Core.Infra.Models;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("SupabaseConnection")));

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

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.RunAsync();