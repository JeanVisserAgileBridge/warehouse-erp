using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public const int UnitPricePrecision = 18;
    public const int UnitPriceScale = 2;

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .ValueGeneratedNever();

        builder.Property(product => product.Sku)
            .HasMaxLength(Product.MaxSkuLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(product => product.Name)
            .HasMaxLength(Product.MaxNameLength)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasMaxLength(Product.MaxDescriptionLength);

        builder.Property(product => product.CategoryId)
            .IsRequired();

        builder.Property(product => product.UnitPrice)
            .HasPrecision(UnitPricePrecision, UnitPriceScale)
            .IsRequired();

        builder.Property(product => product.IsActive)
            .IsRequired();

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        builder.Property(product => product.UpdatedAt)
            .IsRequired();

        builder.HasIndex(product => product.Sku)
            .IsUnique();

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
