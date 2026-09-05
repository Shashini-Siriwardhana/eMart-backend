using CartsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CartsService.Data;
public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
    {
        
    }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Deleting the entire cart should automatically delete its items
        modelBuilder.Entity<Cart>()
        .HasMany(cart => cart.CartItems)
        .WithOne()
        .HasForeignKey(item => item.CartId)
        .OnDelete(DeleteBehavior.Cascade);

        // One cart per user
        modelBuilder.Entity<Cart>()
        .HasIndex(cart => cart.UserId)
        .IsUnique();

        // Same product should appear once per cart
        modelBuilder.Entity<CartItem>()
        .HasIndex(item => new
        {
            item.CartId,
            item.ProductId
        })
        .IsUnique();
    }
}