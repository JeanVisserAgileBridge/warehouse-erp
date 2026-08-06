using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.ProductTests;

public class ProductChangeCategoryTests
{
    [Fact]
    public void ChangeCategory_UpdatesCategoryId()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var newCategoryId = Guid.NewGuid();

        product.ChangeCategory(newCategoryId);

        Assert.Equal(newCategoryId, product.CategoryId);
    }

    [Fact]
    public void ChangeCategory_UpdatesUpdatedAt()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);
        var originalUpdatedAt = product.UpdatedAt;

        product.ChangeCategory(Guid.NewGuid());

        Assert.True(product.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeCategory_RejectsEmptyCategoryId()
    {
        var product = Product.Create("SKU-001", "Widget", Guid.NewGuid(), 9.99m);

        Assert.Throws<DomainException>(() => product.ChangeCategory(Guid.Empty));
    }
}
