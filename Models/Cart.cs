namespace DeliveryShopApp.Models;

public class Cart
{
    public int Id { get; set; }

    //  user_id та session_id nullable для  кошика гостей 
    public int? UserId { get; set; }
    public User? User { get; set; }

    public string? SessionId { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}