using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Data;

namespace DeliveryShopApp.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;

    public HomeController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        // всі категорії
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.SelectedCategory = categoryId;

        // фільтр за категорією
        var productsQuery = _context.Products.Include(p => p.Unit).AsQueryable();
        
        if (categoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
        }

        var products = await productsQuery.ToListAsync();
        return View(products);
    }
}