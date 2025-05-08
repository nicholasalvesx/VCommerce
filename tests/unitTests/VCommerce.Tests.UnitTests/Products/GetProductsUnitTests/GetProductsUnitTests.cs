using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;
using VCommerce.Modules.Core.Application.DTOs;
using Xunit;

namespace VCommerce.Tests.UnitTests.Products.GetProductsUnitTests;

public class GetProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{
    private readonly ProductsController _productsController;

    public GetProductsUnitTests(ProductsUnitTestsController productsController)
    {
        _productsController = new ProductsController(productsController.ProductService);
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
        //arrange
        var prodId = 100;

        //act
        var data = await _productsController.GetProductById(prodId);
        
        //assert
        data.Result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(404);
        
    }

    [Fact]
    public async Task GetProductById_Return_BadRequestResult()
    {
        //arrange 
        var prodId = -1;

        //act
        var data = await _productsController.GetProductById(prodId);
        
        //assert
        data.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProducts_Return_ListOfProductsDto()
    {
        //act
        var data = await _productsController.GetProducts();
        
        //assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeAssignableTo<IEnumerable<ProductDTO>>()
            .And.NotBeNull();
    }

    [Fact]
    public async Task GetProducts_Return_BadRequest()
    {
        //act
        var data = await _productsController.GetProducts();
        
        //assert
        data.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}