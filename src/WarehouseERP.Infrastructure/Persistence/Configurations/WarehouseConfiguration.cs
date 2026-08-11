using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(warehouse => warehouse.Id);

        builder.Property(warehouse => warehouse.Id)
            .ValueGeneratedNever();

        builder.Property(warehouse => warehouse.Code)
            .HasMaxLength(Warehouse.MaxCodeLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(warehouse => warehouse.Name)
            .HasMaxLength(Warehouse.MaxNameLength)
            .IsRequired();

        builder.Property(warehouse => warehouse.Address)
            .HasMaxLength(Warehouse.MaxAddressLength);

        builder.Property(warehouse => warehouse.IsActive)
            .IsRequired();

        builder.Property(warehouse => warehouse.CreatedAt)
            .IsRequired();

        builder.Property(warehouse => warehouse.UpdatedAt)
            .IsRequired();

        builder.HasIndex(warehouse => warehouse.Code)
            .IsUnique();
    }
}
