using VCommerce.Api.Controllers;

namespace VCommerce.Tests.UnitTests.Products.PostProductsUnitTests;

public class PostProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{
    private readonly ProductsController _productsController;
    
    public PostProductsUnitTests(ProductsUnitTestsController productsController)
    {
        _productsController = new ProductsController(productsController.productService);
    }
    
    [Fact]
    public async Task PostProduct_Return_CreatedStatusCode()
    {}
    
    [Fact]
    public async Task PostProduct_Return_BadRequest()
    {}
}