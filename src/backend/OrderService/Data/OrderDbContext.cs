using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Deleting the entire order should automatically delete its items
        modelBuilder.Entity<Order>()
        .HasMany(order => order.OrderItems)
        .WithOne()
        .HasForeignKey(item => item.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

        // Creates a composite unique index - Same product shouldn't appear twice per order.
        modelBuilder.Entity<OrderItem>()
        .HasIndex(item => new
        {
            item.OrderId,
            item.ProductId
        })
        .IsUnique();

        // Index on UserId - Without an index, PostgreSQL may have to scan all orders looking for that user's orders.
        // With an index, PostgreSQL has a faster structure for locating matching rows.
        modelBuilder.Entity<Order>()
        .HasIndex(order => order.UserId);

        // Decimal precision - 18 digits total: 16 before decimal, 2 after decimal
        modelBuilder.Entity<Order>()
        .Property(order => order.TotalAmount)
        .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
        .Property(order => order.UnitPrice)
        .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
        .Property(order => order.SubTotal)
        .HasPrecision(18, 2);
    }
}