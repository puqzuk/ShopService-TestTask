using Microsoft.EntityFrameworkCore;
using ShopService.DataBase.Entities;

namespace ShopService.DataBase;

public class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers;
    public DbSet<Product> Products;
    public DbSet<Purchase> Purchases;
    public DbSet<PurchaseItem> PurchaseItems;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseItem>()
            .HasKey(pi => new { pi.PurchaseId, pi.ProductId });
        
        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.Purchase)
            .WithMany(p => p.Items)
            .HasForeignKey(pi => pi.PurchaseId);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.PurchaseItems)
            .HasForeignKey(pi => pi.ProductId);
    }
}