using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VCommerce.Modules.Core.Application.DTOs.Mappings;
using VCommerce.Modules.Core.Application.Interfaces;
using VCommerce.Modules.Core.Application.Services;
using VCommerce.Modules.Core.Infra;
using VCommerce.Modules.Core.Infra.Repositories;

namespace VCommerce.Tests.UnitTests.Products;

public class ProductsUnitTestsController
{
    public IProductRepository Repository { get; }
    public IMapper Mapper { get; }
    public readonly IProductService ProductService;
    
    private static DbContextOptions<AppDbContext> dbContextOptions { get; }

    private static string connectionString =
        "Server=localhost;Port=5432;Database=vcommerce_db;Username=postgres;Password=qwe123;CommandTimeout=20";

    static ProductsUnitTestsController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString).Options;
    }

    public ProductsUnitTestsController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        
        Mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        Repository = new ProductRepository(context);
        ProductService = new ProductService(Mapper, Repository);
    }
}