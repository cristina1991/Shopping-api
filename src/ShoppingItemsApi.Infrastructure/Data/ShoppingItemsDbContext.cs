using Microsoft.EntityFrameworkCore;
using ShoppingItemsApi.Domain.Entities;

namespace ShoppingItemsApi.Infrastructure.Data;

public class ShoppingItemsDbContext : DbContext
{
    public ShoppingItemsDbContext(DbContextOptions<ShoppingItemsDbContext> options) : base(options)
    {
    }

    public DbSet<ShoppingItem> ShoppingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Price)
                .HasPrecision(18, 2);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);
        });

        base.OnModelCreating(modelBuilder);
    }
}