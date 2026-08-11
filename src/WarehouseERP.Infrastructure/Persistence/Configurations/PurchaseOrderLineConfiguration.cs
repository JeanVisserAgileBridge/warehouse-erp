using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public const int UnitPricePrecision = 18;
    public const int UnitPriceScale = 2;

    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.ToTable("PurchaseOrderLines");

        builder.HasKey(line => line.Id);

        builder.Property(line => line.Id)
            .ValueGeneratedNever();

        builder.Property(line => line.PurchaseOrderId)
            .IsRequired();

        builder.Property(line => line.ProductId)
            .IsRequired();

        builder.Property(line => line.QuantityOrdered)
            .IsRequired();

        builder.Property(line => line.QuantityReceived)
            .IsRequired();

        builder.Property(line => line.UnitPrice)
            .HasPrecision(UnitPricePrecision, UnitPriceScale)
            .IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(line => line.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
