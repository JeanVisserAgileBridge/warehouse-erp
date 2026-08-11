using Microsoft.EntityFrameworkCore;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Procurement;
using WarehouseERP.Domain.Sales;
using WarehouseERP.Domain.Warehouses;
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

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<StorageLocation> StorageLocations => Set<StorageLocation>();

    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        modelBuilder.ApplyConfiguration(new StorageLocationConfiguration());
        modelBuilder.ApplyConfiguration(new StockMovementConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderLineConfiguration());
    }
}
