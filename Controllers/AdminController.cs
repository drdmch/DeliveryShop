using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Data;
using DeliveryShopApp.Models;
using System.Security.Claims;

namespace DeliveryShopApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ProductsCount = await _context.Products.CountAsync();
        ViewBag.OrdersCount = await _context.Orders.CountAsync();
        ViewBag.UsersCount = await _context.Users.CountAsync();
        ViewBag.Statuses = await _context.OrderStatuses.ToListAsync();

        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.CurrentStatus)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Products()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Units = await _context.Units.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        product.IsAvailable = true;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Products");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProductValues(int id, decimal price, int stockQuantity)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            product.Price = price;
            product.StockQuantity = stockQuantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Products");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Products");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, int statusId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        var adminIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out int adminId))
        {
            return Challenge();
        }

        order.CurrentStatusId = statusId;
        order.UpdatedAt = DateTime.UtcNow;

        var history = new OrderStatusHistory
        {
            OrderId = orderId,
            StatusId = statusId,
            ChangedById = adminId,
            ChangedAt = DateTime.UtcNow
        };

        _context.OrderStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}