using Microsoft.EntityFrameworkCore;
using KitchenService.Domain.Entities;
using KitchenService.Infrastructure.Outbox;

namespace KitchenService.Infrastructure.Data;

public class KitchenDbContext : DbContext
{
    public KitchenDbContext(DbContextOptions<KitchenDbContext> options) : base(options) { }

    public DbSet<KitchenOrder> KitchenOrders { get; set; }
    public DbSet<KitchenOrderItem> KitchenOrderItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitchenOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.HasMany(e => e.OrderItems)
                  .WithOne(oi => oi.KitchenOrder)
                  .HasForeignKey(oi => oi.KitchenOrderId);
        });

        modelBuilder.Entity<KitchenOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MenuItemName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SpecialInstructions).HasMaxLength(500);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.OccurredOnUtc).IsRequired();
        });
    }
}