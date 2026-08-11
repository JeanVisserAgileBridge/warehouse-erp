using Microsoft.EntityFrameworkCore;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Procurement;
using WarehouseERP.Infrastructure.Persistence.Configurations;

namespace WarehouseERP.Infrastructure.Persistence;

public class WarehouseErpDbContext : DbContext
{
    public WarehouseErpDbContext(DbContextOptions<WarehouseErpDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
    }
}
