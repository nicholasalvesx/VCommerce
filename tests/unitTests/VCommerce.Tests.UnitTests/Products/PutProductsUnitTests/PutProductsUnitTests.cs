using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.Controllers;
using VCommerce.Api.DTOs;

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
    {
        //arrange
        var prodId = 5;

        var updateProductDto = new ProductDTO
        {
            Id = prodId,
            Name = "Produto atualizado",
            Description = "Produto atualizado",
            ImageUrl = "atualizado.jpeg",
            CategoryId = 6
        };
        
        //act 
        var result = await _productsController.UpdateProduct(prodId, updateProductDto);
        
        //assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PutProducts_Update_Return_BadRequest()
    {
        //arrange
        var prodId = 1000;
        
        var updateProductDto = new ProductDTO
        {
            Id = 16,
            Name = "Produto atualizado",
            Description = "Produto atualizado",
            ImageUrl = "atualizado.jpeg",
            CategoryId = 6
        };
        
        //act
        var data = await _productsController.UpdateProduct(prodId, updateProductDto);
        
        //assert 
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }
}