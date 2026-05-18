namespace DeliveryShopApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}