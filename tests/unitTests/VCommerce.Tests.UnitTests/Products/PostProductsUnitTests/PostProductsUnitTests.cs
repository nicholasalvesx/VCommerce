using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;
using VCommerce.Modules.Core.Application.DTOs;
using Xunit;

namespace VCommerce.Tests.UnitTests.Products.PostProductsUnitTests;

public class PostProductsUnitTests : IClassFixture<ProductsUnitTestsController>
{
    private readonly ProductsController _productsController;
    
    public PostProductsUnitTests(ProductsUnitTestsController productsController)
    {
        _productsController = new ProductsController(productsController.ProductService);
    }

    [Fact]
    public async Task PostProduct_Return_CreatedStatusCode()
    {
        //arrange 
        var product = new ProductDTO
        {
            Name = "Novo produto",
            Description = "Novo produto",
            Price = 100,
            CategoryId = 5
        };
        
        //act
        var data = await _productsController.CreateProduct(product);
        
        //assert
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduct_Return_BadRequest()
    {
        //arrange 
        ProductDTO? product = null;
        
        //act 
        var data = await _productsController.CreateProduct(product);
        
        //assert 
        var badRequestResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}