using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.CategoryTests;

public class CategoryActivationTests
{
    [Fact]
    public void Deactivate_MakesCategoryInactive()
    {
        var category = Category.Create("Beverages");

        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var category = Category.Create("Beverages");

        category.Deactivate();
        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact]
    public void Activate_MakesInactiveCategoryActive()
    {
        var category = Category.Create("Beverages");
        category.Deactivate();

        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var category = Category.Create("Beverages");

        category.Activate();
        category.Activate();

        Assert.True(category.IsActive);
    }
}
