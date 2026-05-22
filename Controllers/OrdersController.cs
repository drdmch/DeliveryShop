using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Data;
using DeliveryShopApp.Models;
using System.Security.Claims;

namespace DeliveryShopApp.Controllers;

public class OrdersController : Controller
{
    private readonly DataContext _context;

    public OrdersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    return RedirectToAction("MyOrders", new { searchKey = user.Email });
                }
            }
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders(string searchKey)
    {
        if (string.IsNullOrEmpty(searchKey))
        {
            return RedirectToAction("Index");
        }

        var orders = await _context.Orders
            .Include(o => o.CurrentStatus)
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .Where(o => o.User.Phone == searchKey || o.User.Email == searchKey)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        ViewBag.SearchKey = searchKey;

        var productIdsWithReviews = await _context.Reviews
            .Where(r => r.User.Phone == searchKey || r.User.Email == searchKey)
            .Select(r => r.ProductId)
            .ToListAsync();
            
        ViewBag.LeftReviewProductIds = productIdsWithReviews;

        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> SearchOrders(string searchKey)
    {
        return RedirectToAction("MyOrders", new { searchKey });
    }

    [HttpPost]
    public async Task<IActionResult> LeaveOrderReview(int productId, int orderId, string searchKey, string comment, int rating)
    {
        var order = await _context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null) return RedirectToAction("Index");

        var review = new Review
        {
            ProductId = productId,
            OrderId = orderId,
            UserId = order.UserId,
            AuthorName = $"{order.User.FirstName} {order.User.LastName}",
            Comment = comment,
            Rating = rating < 1 ? 1 : (rating > 5 ? 5 : rating),
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyOrders", new { searchKey });
    }
}