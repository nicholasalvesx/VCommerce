using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;
using VCommerce.Api.Models;
using Xunit;

namespace VCommerce.Tests.UnitTests.Products.DeleteProductsUnitTests;

public class DeleteProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{ 
    private readonly ProductsController _controller;

    public DeleteProductsUnitTests(ProductsUnitTestsController controller)
    {
        _controller = new ProductsController(controller.productService);
    }

    [Fact]
    public async Task DeleteProduct_Return_OkResult()
    {
        //arrange 
        const int prodId = 5;

        //act
        var data = await _controller.DeleteProduct(prodId);

        //assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteProduct_Return_NotFoundResult()
    {
        //arrange
        const int prodId = 1000;

        //act
        var data = await _controller.DeleteProduct(prodId);
        
        //assert
        data.Result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(404);
    }
}