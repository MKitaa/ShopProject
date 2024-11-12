using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.DTOs.product;
using Shop.Interfaces;
using Shop.Models;

namespace Shop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts()
    {
        var products = await productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
    {
        var product = await productService.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var product = await productService.CreateProduct(createProductDto);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int id,
        [FromBody] UpdateProductDto updateProductDto)
    {
        var product = await productService.UpdateProduct(id, updateProductDto);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        var success = await productService.DeleteProduct(id);
        if (!success) NotFound();
        return NoContent();
    }

    [HttpGet("filter/{minPrice}/{maxPrice}")]
    public async Task<IActionResult> GetProductsByPriceRange([FromRoute] decimal minPrice, [FromRoute] decimal maxPrice)
    {
        var products = await productService.GetProductsByPriceRangeUsingSubquery(minPrice, maxPrice);
        return Ok(products);
    }
}
