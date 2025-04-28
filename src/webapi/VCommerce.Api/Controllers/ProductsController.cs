using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.DTOs;
using VCommerce.Api.Services;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
    {
        var productsDto = await _productService.GetProducts();
        return Ok(productsDto);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO? product)
    {
        if (product == null)
        {
            return BadRequest();
        }
        
        await _productService.AddProduct(product);
        
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, ProductDTO product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        await _productService.UpdateProduct(product);
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> DeleteProduct(int id)
    {
        var productDto = await _productService.GetProductById(id);
        if (productDto == null!)
        {
            return NotFound("Nenhum produto foi encontrado");
        }
        await _productService.DeleteProduct(id);
        return Ok(productDto);
    }
}