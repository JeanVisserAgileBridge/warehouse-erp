using Microsoft.EntityFrameworkCore;
using WarehouseERP.Domain.ProductCatalog;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}
