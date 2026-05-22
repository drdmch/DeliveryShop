using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Data;
using DeliveryShopApp.Models;

namespace DeliveryShopApp.Controllers;

[Route("api/Products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly DataContext _context;

    public ProductsApiController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetProducts()
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .Select(p => new 
            {
                p.Id,
                p.Name,
                p.Price,
                p.StockQuantity
            })
            .ToListAsync();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Price = updatedProduct.Price;
        product.StockQuantity = updatedProduct.StockQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}