using Microsoft.AspNetCore.Identity;
using VCommerce.Modules.Core.Infra.Models;

namespace WorkerService;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

     public async Task StartAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Identity Migration Worker inicializando...");
            
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            
            using var scope = _serviceProvider.CreateScope();
            
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            string[] roles = [
                "Admin",
                "Client"
            ];
            
            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role)) continue;
                
                await roleManager.CreateAsync(new IdentityRole(role));
                _logger.LogInformation("Role {Role} criada com sucesso", role);
            }
            
            var leader = new ApplicationUser
            {
                UserName = "admin",
                Email = "leader@vcommerce.com.br",
                EmailConfirmed = true,
            };

            if (await userManager.FindByEmailAsync(leader.Email) == null)
            {
                var result = await userManager.CreateAsync(leader, "@Qwe123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(leader, "Admin");
                    _logger.LogInformation("Usuário administrador criado com sucesso");
                }
                else
                {
                    _logger.LogError("Erro ao criar a role de administrador");
                }
            }
            
            _logger.LogInformation("Processo concluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no processo");
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}