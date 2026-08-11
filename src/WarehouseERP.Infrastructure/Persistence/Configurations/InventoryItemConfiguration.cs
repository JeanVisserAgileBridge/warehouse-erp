using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems");

        builder.HasKey(inventoryItem => inventoryItem.Id);

        builder.Property(inventoryItem => inventoryItem.Id)
            .ValueGeneratedNever();

        builder.Property(inventoryItem => inventoryItem.ProductId)
            .IsRequired();

        builder.Property(inventoryItem => inventoryItem.StorageLocationId)
            .IsRequired();

        builder.Property(inventoryItem => inventoryItem.QuantityOnHand)
            .IsRequired();

        builder.Property(inventoryItem => inventoryItem.ReorderLevel)
            .IsRequired();

        builder.Property(inventoryItem => inventoryItem.UpdatedAt)
            .IsRequired();

        builder.HasIndex(inventoryItem => new { inventoryItem.ProductId, inventoryItem.StorageLocationId })
            .IsUnique();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(inventoryItem => inventoryItem.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<StorageLocation>()
            .WithMany()
            .HasForeignKey(inventoryItem => inventoryItem.StorageLocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
