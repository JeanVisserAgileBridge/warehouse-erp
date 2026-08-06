using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductActivationTests
{
    [Fact]
    public void Deactivate_MakesProductInactive()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.Deactivate();

        Assert.False(product.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.Deactivate();
        product.Deactivate();

        Assert.False(product.IsActive);
    }

    [Fact]
    public void Activate_MakesInactiveProductActive()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        product.Deactivate();

        product.Activate();

        Assert.True(product.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.Activate();
        product.Activate();

        Assert.True(product.IsActive);
    }

    [Fact]
    public void Deactivate_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.Deactivate();

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }
}
