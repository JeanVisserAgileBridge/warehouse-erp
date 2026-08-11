using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(stockMovement => stockMovement.Id);

        builder.Property(stockMovement => stockMovement.Id)
            .ValueGeneratedNever();

        builder.Property(stockMovement => stockMovement.InventoryItemId)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.MovementType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(stockMovement => stockMovement.Quantity)
            .IsRequired();

        builder.Property(stockMovement => stockMovement.Reference)
            .HasMaxLength(StockMovement.MaxReferenceLength);

        builder.Property(stockMovement => stockMovement.OccurredAt)
            .IsRequired();

        builder.HasOne<InventoryItem>()
            .WithMany()
            .HasForeignKey(stockMovement => stockMovement.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
