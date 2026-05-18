using Microsoft.EntityFrameworkCore;
using DeliveryShopApp.Models;

namespace DeliveryShopApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Unit>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Product_StockQuantity", "\"StockQuantity\" >= 0"));

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Cart_Owner", "\"UserId\" IS NOT NULL OR \"SessionId\" IS NOT NULL"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Cart_Quantity", "\"Quantity\" > 0"));

            entity.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_DeliveryFee", "\"DeliveryFee\" >= 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Order_TotalPrice", "\"TotalPrice\" >= 0"));

            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.CurrentStatus)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.CurrentStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "\"Quantity\" > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_OrderItem_PriceAtPurchase", "\"PriceAtPurchase\" >= 0"));

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Ignore(oi => oi.TotalPrice);
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasOne(h => h.Order)
                .WithMany(o => o.StatusHistories)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Status)
                .WithMany(s => s.StatusHistories)
                .HasForeignKey(h => h.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.ChangedBy)
                .WithMany(u => u.StatusHistories)
                .HasForeignKey(h => h.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "\"Rating\" >= 1 AND \"Rating\" <= 5"));

            entity.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Order)
                .WithMany(o => o.Reviews)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}