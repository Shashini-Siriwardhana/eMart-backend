using Microsoft.EntityFrameworkCore;
using PaymentsService.Models;

namespace PaymentsService.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
        
    }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>()
            .Property(payment => payment.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .HasIndex(payment => payment.OrderId);
    }
}