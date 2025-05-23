using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VCommerce.Modules.Core.Infra;
using VCommerce.Modules.Core.Infra.Models;
using VCommerce.Mvc.Configuration;
using VCommerce.Mvc.Services;
using VCommerce.Mvc.Services.Contracts;

AppContext.SetSwitch("Npgsql.EnablePrepare", false);
var builder = WebApplication.CreateBuilder(args);

var keyStorageLocation = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "protection-keys");

Directory.CreateDirectory(keyStorageLocation);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyStorageLocation))
    .SetApplicationName("VCommerce.Mvc");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.AddHttpClient("Api", client =>
    {
        var apiUrl = Environment.GetEnvironmentVariable("API_URL") ??
                      builder.Configuration["ServiceUri:Api"] ??
                      "http://localhost:5000";                         
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    })
    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ??
                            builder.Configuration.GetConnectionString("SupabaseConnection");                              
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/auth/logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Register}/{id?}")
    .WithStaticAssets();

app.Run();