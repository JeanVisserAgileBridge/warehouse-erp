using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductChangeDescriptionTests
{
    [Fact]
    public void ChangeDescription_UpdatesDescription()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, "Old description");

        product.ChangeDescription("New description");

        Assert.Equal("New description", product.Description);
    }

    [Fact]
    public void ChangeDescription_ClearsDescriptionWhenNull()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, "Old description");

        product.ChangeDescription(null);

        Assert.Null(product.Description);
    }

    [Fact]
    public void ChangeDescription_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.ChangeDescription("New description");

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeDescription_RejectsDescriptionLongerThanMaxLength()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var description = new string('a', Product.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => product.ChangeDescription(description));
    }
}
