namespace DeliveryShopApp.Models;

public class Order
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public int CurrentStatusId { get; set; }
    public OrderStatus CurrentStatus { get; set; } = null!;

    public decimal DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}