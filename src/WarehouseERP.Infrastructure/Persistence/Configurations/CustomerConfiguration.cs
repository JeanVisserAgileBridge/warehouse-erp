using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    // TODO: Reference Customer.MaxEmailLength instead of this literal if that domain constant is added.
    public const int EmailMaxLength = 256;

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Id)
            .ValueGeneratedNever();

        builder.Property(customer => customer.Name)
            .HasMaxLength(Customer.MaxNameLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(customer => customer.Email)
            .HasMaxLength(EmailMaxLength);

        builder.Property(customer => customer.PhoneNumber)
            .HasMaxLength(Customer.MaxPhoneNumberLength);

        builder.Property(customer => customer.Address)
            .HasMaxLength(Customer.MaxAddressLength);

        builder.Property(customer => customer.IsActive)
            .IsRequired();

        builder.Property(customer => customer.CreatedAt)
            .IsRequired();

        builder.Property(customer => customer.UpdatedAt)
            .IsRequired();

        builder.HasIndex(customer => customer.Name)
            .IsUnique();
    }
}
