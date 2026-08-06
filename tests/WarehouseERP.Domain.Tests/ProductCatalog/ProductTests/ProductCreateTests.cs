using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductCreateTests
{
    [Fact]
    public void Create_ReturnsProductWithNonEmptyGuid()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var categoryId = Guid.NewGuid();

        var product = Product.Create("SKU-001", "Widget", categoryId, 9.99m, "A useful widget");

        Assert.Equal("SKU-001", product.Sku);
        Assert.Equal("Widget", product.Name);
        Assert.Equal(categoryId, product.CategoryId);
        Assert.Equal(9.99m, product.UnitPrice);
        Assert.Equal("A useful widget", product.Description);
    }

    [Fact]
    public void Create_MakesProductActive()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.True(product.IsActive);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.Equal(product.CreatedAt, product.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullDescription()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, null);

        Assert.Null(product.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceSku(string? sku)
    {
        Assert.Throws<DomainException>(() => Product.Create(sku!, "Widget", Guid.NewGuid(), 9.99m));
    }

    [Fact]
    public void Create_RejectsSkuLongerThanMaxLength()
    {
        var sku = new string('a', Product.MaxSkuLength + 1);

        Assert.Throws<DomainException>(() => Product.Create(sku, "Widget", Guid.NewGuid(), 9.99m));
    }

    [Fact]
    public void Create_AcceptsSkuAtMaxLength()
    {
        var sku = new string('a', Product.MaxSkuLength);

        var product = Product.Create(sku, "Widget", Guid.NewGuid(), 9.99m);

        Assert.Equal(sku, product.Sku);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        Assert.Throws<DomainException>(() => Product.Create("SKU-001", name!, Guid.NewGuid(), 9.99m));
    }

    [Fact]
    public void Create_RejectsNameLongerThanMaxLength()
    {
        var name = new string('a', Product.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => Product.Create("SKU-001", name, Guid.NewGuid(), 9.99m));
    }

    [Fact]
    public void Create_AcceptsNameAtMaxLength()
    {
        var name = new string('a', Product.MaxNameLength);

        var product = Product.Create("SKU-001", name, Guid.NewGuid(), 9.99m);

        Assert.Equal(name, product.Name);
    }

    [Fact]
    public void Create_RejectsDescriptionLongerThanMaxLength()
    {
        var description = new string('a', Product.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, description));
    }

    [Fact]
    public void Create_AcceptsDescriptionAtMaxLength()
    {
        var description = new string('a', Product.MaxDescriptionLength);

        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m, description);

        Assert.Equal(description, product.Description);
    }

    [Fact]
    public void Create_RejectsEmptyCategoryId()
    {
        Assert.Throws<DomainException>(() => Product.Create("SKU-001", "Widget", Guid.Empty, 9.99m));
    }

    [Fact]
    public void Create_RejectsNegativeUnitPrice()
    {
        Assert.Throws<DomainException>(() => Product.Create("SKU-001", "Widget", Guid.NewGuid(), -0.01m));
    }

    [Fact]
    public void Create_AcceptsZeroUnitPrice()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 0m);

        Assert.Equal(0m, product.UnitPrice);
    }
}
