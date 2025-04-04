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
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var productsDto = await _productService.GetProducts();
        if (productsDto == null!)
        {
            return BadRequest("No products found");
        }
        return Ok(productsDto);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> Get(int id)
    {
        var productDto = await _productService.GetProductById(id);
        if (productDto == null!)
        {
            return BadRequest("No products found");            
        }
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
    {
        if (productDto == null!)
        {
            return BadRequest();
        }
        
        await _productService.AddProduct(productDto);
        
        return CreatedAtRoute("GetProduct", new { id = productDto.Id }, productDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
    {
        if (id != productDto.Id)
        {
            return BadRequest();
        }

        if (productDto == null!)
        {
            return BadRequest();
        }
        await _productService.UpdateProduct(productDto);
        return Ok(productDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var productDto = await _productService.GetProductById(id);
        if (productDto == null!)
        {
            return BadRequest();
        }
        await _productService.DeleteProduct(id);
        return Ok(productDto);
    }
}