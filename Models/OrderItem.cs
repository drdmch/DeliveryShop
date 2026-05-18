namespace DeliveryShopApp.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    //  product_id є nullable, щоб видалення товару з каталогу не руйнувало історію замовлень
    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    // копіювання текстових даних на момент покупки
    public string ProductName { get; set; } = null!;
    public string UnitName { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }

    public decimal TotalPrice => Quantity * PriceAtPurchase;
}