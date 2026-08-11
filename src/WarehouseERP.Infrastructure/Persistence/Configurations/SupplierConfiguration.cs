using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    // TODO: Reference Supplier.MaxEmailLength instead of this literal if that domain constant is added.
    public const int EmailMaxLength = 256;

    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(supplier => supplier.Id);

        builder.Property(supplier => supplier.Id)
            .ValueGeneratedNever();

        builder.Property(supplier => supplier.Name)
            .HasMaxLength(Supplier.MaxNameLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(supplier => supplier.Email)
            .HasMaxLength(EmailMaxLength);

        builder.Property(supplier => supplier.PhoneNumber)
            .HasMaxLength(Supplier.MaxPhoneNumberLength);

        builder.Property(supplier => supplier.Address)
            .HasMaxLength(Supplier.MaxAddressLength);

        builder.Property(supplier => supplier.IsActive)
            .IsRequired();

        builder.Property(supplier => supplier.CreatedAt)
            .IsRequired();

        builder.Property(supplier => supplier.UpdatedAt)
            .IsRequired();

        builder.HasIndex(supplier => supplier.Name)
            .IsUnique();
    }
}
