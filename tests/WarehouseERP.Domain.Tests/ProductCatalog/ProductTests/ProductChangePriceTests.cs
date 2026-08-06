using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductChangePriceTests
{
    [Fact]
    public void ChangePrice_UpdatesUnitPrice()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.ChangePrice(19.99m);

        Assert.Equal(19.99m, product.UnitPrice);
    }

    [Fact]
    public void ChangePrice_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.ChangePrice(19.99m);

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangePrice_AcceptsZero()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.ChangePrice(0m);

        Assert.Equal(0m, product.UnitPrice);
    }

    [Fact]
    public void ChangePrice_RejectsNegativeUnitPrice()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.Throws<DomainException>(() => product.ChangePrice(-0.01m));
    }
}
