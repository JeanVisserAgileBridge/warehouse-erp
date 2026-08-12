using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrders");

        builder.HasKey(salesOrder => salesOrder.Id);

        builder.Property(salesOrder => salesOrder.Id)
            .ValueGeneratedNever();

        builder.Property(salesOrder => salesOrder.CustomerId)
            .IsRequired();

        builder.Property(salesOrder => salesOrder.OrderNumber)
            .HasMaxLength(SalesOrder.MaxOrderNumberLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(salesOrder => salesOrder.OrderDate)
            .IsRequired();

        builder.Property(salesOrder => salesOrder.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(salesOrder => salesOrder.Notes)
            .HasMaxLength(SalesOrder.MaxNotesLength);

        builder.Property(salesOrder => salesOrder.CreatedAt)
            .IsRequired();

        builder.Property(salesOrder => salesOrder.UpdatedAt)
            .IsRequired();

        builder.HasIndex(salesOrder => salesOrder.OrderNumber)
            .IsUnique();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(salesOrder => salesOrder.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // SalesOrderLine is a child entity of the SalesOrder aggregate: the collection is
        // mapped to the private _lines backing field so that EF Core's change tracker observes
        // additions/removals made through SalesOrder.AddLine/RemoveLine, without exposing a
        // mutable collection on the aggregate's public API.
        builder.HasMany(salesOrder => salesOrder.Lines)
            .WithOne()
            .HasForeignKey(line => line.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(salesOrder => salesOrder.Lines)
            .HasField("_lines")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
