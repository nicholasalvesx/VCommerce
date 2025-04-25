using VCommerce.Api.Controllers;

namespace VCommerce.Tests.UnitTests.Products.PutProductsUnitTests;

public class PutProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{
    private readonly ProductsController _productsController;

    public PutProductsUnitTests(ProductsUnitTestsController productsController)
    {
        _productsController = new ProductsController(productsController.productService);
    }
    
    [Fact]
    public async Task PutProducts_Update_Return_OkResult()
    {} 
    
    [Fact]
    public async Task PutProducts_Update_Return_BadRequest()
    {}
}