using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductRenameTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        product.Rename("Gadget");

        Assert.Equal("Gadget", product.Name);
    }

    [Fact]
    public void Rename_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.Rename("Gadget");

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.Throws<DomainException>(() => product.Rename(name!));
    }

    [Fact]
    public void Rename_RejectsNameLongerThanMaxLength()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var name = new string('a', Product.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => product.Rename(name));
    }
}
