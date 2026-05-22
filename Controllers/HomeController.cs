using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Data;
using DeliveryShopApp.Models;

namespace DeliveryShopApp.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;

    public HomeController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId, string searchString)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.SearchString = searchString;

        var productsQuery = _context.Products.Include(p => p.Unit).AsQueryable();

        if (categoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            var selectedCat = categories.FirstOrDefault(c => c.Id == categoryId.Value);
            ViewBag.CategoryName = selectedCat?.Name;
        }
        else
        {
            ViewBag.CategoryName = "Всі товари";
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            ViewBag.CategoryName = $"Пошук: \"{searchString}\"";
        }

        var products = await productsQuery.ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Unit)
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        ViewBag.AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0;
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(int productId, string authorName, string comment, int rating)
    {
        var review = new Review
            {
                ProductId = productId,
                AuthorName = string.IsNullOrEmpty(authorName) ? "Анонім" : authorName,
                Comment = comment,
                Rating = rating < 1 ? 1 : (rating > 5 ? 5 : rating),
                CreatedAt = DateTime.UtcNow
            };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = productId });
    }
}