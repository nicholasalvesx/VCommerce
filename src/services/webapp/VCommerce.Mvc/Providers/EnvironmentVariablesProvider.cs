namespace VCommerce.Mvc.Providers;

public class EnvironmentVariablesProvider 
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public EnvironmentVariablesProvider(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public string? GetDatabaseConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
            
        if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
        {
            connectionString = ConvertPostgresUrlToConnectionString(connectionString);
        }
            
        return connectionString;
    }
        
    public string? GetApiBaseUrl()
    {
        var apiUrl = Environment.GetEnvironmentVariable("API_URL");
            
        if (string.IsNullOrEmpty(apiUrl))
        {
            apiUrl = _configuration["ServiceUri:Api"];
        }
            
        return apiUrl;
    }
        
    public bool IsProduction()
    {
        return _environment.IsProduction() || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER"));
    }
        
    private static string ConvertPostgresUrlToConnectionString(string postgresUrl)
    {
        try
        {
            var uri = new Uri(postgresUrl);
            var userInfo = uri.UserInfo.Split(':');
            var username = userInfo[0];
            var password = userInfo.Length > 1 ? userInfo[1] : string.Empty;
            var host = uri.Host;
            var port = uri.Port > 0 ? uri.Port : 5432;
            var database = uri.AbsolutePath.TrimStart('/');
                
            return $"Server={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
        }
        catch (Exception)
        {
            return postgresUrl;
        }
    }
}