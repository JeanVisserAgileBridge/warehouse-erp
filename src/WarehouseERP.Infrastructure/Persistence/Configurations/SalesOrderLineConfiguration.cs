using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.ProductCatalog;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public const int UnitPricePrecision = 18;
    public const int UnitPriceScale = 2;

    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLines");

        builder.HasKey(line => line.Id);

        builder.Property(line => line.Id)
            .ValueGeneratedNever();

        builder.Property(line => line.SalesOrderId)
            .IsRequired();

        builder.Property(line => line.ProductId)
            .IsRequired();

        builder.Property(line => line.QuantityOrdered)
            .IsRequired();

        builder.Property(line => line.QuantityFulfilled)
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
