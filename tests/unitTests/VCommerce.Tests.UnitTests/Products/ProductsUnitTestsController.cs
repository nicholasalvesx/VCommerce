using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Data;
using VCommerce.Api.DTOs.Mappings;
using VCommerce.Api.Repositores;
using VCommerce.Api.Services;

namespace VCommerce.Tests.UnitTests.Products;

public class ProductsUnitTestsController
{
    public IProductRepository Repository { get; }
    public IMapper Mapper { get; }
    public IProductService productService;
    
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
        productService = new ProductService(Mapper, Repository);
    }
}