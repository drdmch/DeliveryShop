using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DeliveryShopApp.Data;
using DeliveryShopApp.Models;

namespace DeliveryShopApp.Controllers;

public class CartController : Controller
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartController(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetOrCreateSessionId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return string.Empty;

        var cookieKey = "DeliveryCartSessionId";
        if (context.Request.Cookies.TryGetValue(cookieKey, out var sessionId) && !string.IsNullOrEmpty(sessionId))
        {
            return sessionId;
        }

        var newSessionId = Guid.NewGuid().ToString();

        if (!context.Response.HasStarted)
        {
            context.Response.Cookies.Append(cookieKey, newSessionId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true
            });
        }

        return newSessionId;
    }

    public async Task<IActionResult> Index()
    {
        var sessionId = GetOrCreateSessionId();
        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .ThenInclude(p => p.Unit)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, decimal quantity = 1)
    {
        var sessionId = GetOrCreateSessionId();
        var product = await _context.Products.FindAsync(productId);

        if (product == null || !product.IsAvailable || product.StockQuantity < 1)
        {
            return RedirectToAction("Index", "Home");
        }

        var cartItem = await _context.Carts
            .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new Cart
            {
                SessionId = sessionId,
                ProductId = productId,
                Quantity = quantity,
                AddedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SetQuantity(int cartId, decimal quantity)
    {
        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            if (quantity <= 0)
            {
                _context.Carts.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartId, decimal change)
    {
        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            cartItem.Quantity += change;
            if (cartItem.Quantity <= 0)
            {
                _context.Carts.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartId)
    {
        var cartItem = await _context.Carts.FindAsync(cartId);
        if (cartItem != null)
        {
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var sessionId = GetOrCreateSessionId();
        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        ViewBag.TotalSum = cartItems.Sum(item => item.Quantity * item.Product.Price);

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    ViewBag.UserFirstName = user.FirstName;
                    ViewBag.UserLastName = user.LastName;
                    ViewBag.UserPhone = user.Phone;
                    ViewBag.UserEmail = user.Email;
                }
            }
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder(string firstName, string lastName, string phone, string email, string address, string comment)
    {
        var sessionId = GetOrCreateSessionId();

        if (string.IsNullOrEmpty(sessionId))
        {
            return RedirectToAction("Index", "Home");
        }

        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .ThenInclude(p => p.Unit)
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email || u.Phone == phone);

        if (user == null)
        {
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Client");
            if (defaultRole == null)
            {
                defaultRole = new Role { Name = "Client" };
                _context.Roles.Add(defaultRole);
                await _context.SaveChangesAsync();
            }

            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Email = email,
                PasswordHash = Guid.NewGuid().ToString(),
                RoleId = defaultRole.Id
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var defaultStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == 1 || s.Name == "Нове");
        if (defaultStatus == null)
        {
            defaultStatus = new OrderStatus { Name = "Нове", SortOrder = 10 };
            _context.OrderStatuses.Add(defaultStatus);
            await _context.SaveChangesAsync();
        }

        decimal totalSum = cartItems.Sum(item => item.Quantity * item.Product.Price);

        var order = new Order
        {
            UserId = user.Id,
            DeliveryAddress = address,
            CurrentStatusId = defaultStatus.Id,
            DeliveryFee = 0,
            TotalPrice = totalSum,
            Comment = comment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in cartItems)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitName = item.Product.Unit?.Name ?? "шт",
                Quantity = item.Quantity,
                PriceAtPurchase = item.Product.Price
            };
            _context.OrderItems.Add(orderItem);

            if (item.Product.StockQuantity >= item.Quantity)
            {
                item.Product.StockQuantity -= (int)item.Quantity;
            }
        }

        _context.Carts.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return View("OrderSuccess", order.Id);
    }
}