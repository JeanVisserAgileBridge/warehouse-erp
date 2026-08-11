using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("StorageLocations");

        builder.HasKey(storageLocation => storageLocation.Id);

        builder.Property(storageLocation => storageLocation.Id)
            .ValueGeneratedNever();

        builder.Property(storageLocation => storageLocation.WarehouseId)
            .IsRequired();

        builder.Property(storageLocation => storageLocation.Code)
            .HasMaxLength(StorageLocation.MaxCodeLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(storageLocation => storageLocation.Description)
            .HasMaxLength(StorageLocation.MaxDescriptionLength);

        builder.Property(storageLocation => storageLocation.IsActive)
            .IsRequired();

        builder.Property(storageLocation => storageLocation.CreatedAt)
            .IsRequired();

        builder.Property(storageLocation => storageLocation.UpdatedAt)
            .IsRequired();

        builder.HasIndex(storageLocation => new { storageLocation.WarehouseId, storageLocation.Code })
            .IsUnique();

        builder.HasOne<Warehouse>()
            .WithMany()
            .HasForeignKey(storageLocation => storageLocation.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
