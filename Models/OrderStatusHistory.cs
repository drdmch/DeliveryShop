namespace DeliveryShopApp.Models;

public class OrderStatusHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int StatusId { get; set; }
    public OrderStatus Status { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public int ChangedById { get; set; }
    public User ChangedBy { get; set; } = null!;
}