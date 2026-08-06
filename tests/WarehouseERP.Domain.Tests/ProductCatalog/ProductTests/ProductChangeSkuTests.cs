using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductChangeSkuTests
{
    [Fact]
    public void ChangeSku_UpdatesSku()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.ChangeSku("SKU-002");

        Assert.Equal("SKU-002", product.Sku);
    }

    [Fact]
    public void ChangeSku_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.ChangeSku("SKU-002");

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeSku_RejectsNullEmptyOrWhitespaceSku(string? sku)
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.Throws<DomainException>(() => product.ChangeSku(sku!));
    }

    [Fact]
    public void ChangeSku_RejectsSkuLongerThanMaxLength()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var sku = new string('a', Product.MaxSkuLength + 1);

        Assert.Throws<DomainException>(() => product.ChangeSku(sku));
    }
}
