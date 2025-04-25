using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;

namespace VCommerce.Tests.UnitTests.Products.GetProductsUnitTests;

public class GetProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{
    private readonly ProductsController _productsController;

    public GetProductsUnitTests(ProductsUnitTestsController productsController)
    {
        _productsController = new ProductsController(productsController.productService);
    }

    [Fact]
    public async Task GetProductById_Return_OkResult()
    {
        //arrange
        var prodId = 5;
        
        //act
        var data = await _productsController.GetProductById(prodId);
        
        //assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProductById_Return_NotFoundResult()
    {
    }

    [Fact]
    public async Task GetProductById_Return_BadRequestResult()
    {
    }

    [Fact]
    public async Task GetProducts_Return_ListOfProductsDto()
    {
    }

    [Fact]
    public async Task GetProducts_Return_BadRequest()
    {
    }
}