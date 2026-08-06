using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(Category.MaxNameLength)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(category => category.Description)
            .HasMaxLength(Category.MaxDescriptionLength);

        builder.Property(category => category.IsActive)
            .IsRequired();

        builder.HasIndex(category => category.Name)
            .IsUnique();
    }
}
