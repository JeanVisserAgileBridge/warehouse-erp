using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(purchaseOrder => purchaseOrder.Id);

        builder.Property(purchaseOrder => purchaseOrder.Id)
            .ValueGeneratedNever();

        builder.Property(purchaseOrder => purchaseOrder.SupplierId)
            .IsRequired();

        builder.Property(purchaseOrder => purchaseOrder.OrderNumber)
            .HasMaxLength(PurchaseOrder.MaxOrderNumberLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(purchaseOrder => purchaseOrder.OrderDate)
            .IsRequired();

        builder.Property(purchaseOrder => purchaseOrder.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(purchaseOrder => purchaseOrder.Notes)
            .HasMaxLength(PurchaseOrder.MaxNotesLength);

        builder.Property(purchaseOrder => purchaseOrder.CreatedAt)
            .IsRequired();

        builder.Property(purchaseOrder => purchaseOrder.UpdatedAt)
            .IsRequired();

        builder.HasIndex(purchaseOrder => purchaseOrder.OrderNumber)
            .IsUnique();

        builder.HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(purchaseOrder => purchaseOrder.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseOrderLine is a child entity of the PurchaseOrder aggregate: the collection is
        // mapped to the private _lines backing field so that EF Core's change tracker observes
        // additions/removals made through PurchaseOrder.AddLine/RemoveLine, without exposing a
        // mutable collection on the aggregate's public API.
        builder.HasMany(purchaseOrder => purchaseOrder.Lines)
            .WithOne()
            .HasForeignKey(line => line.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(purchaseOrder => purchaseOrder.Lines)
            .HasField("_lines")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
