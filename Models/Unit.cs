namespace DeliveryShopApp.Models;

public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}