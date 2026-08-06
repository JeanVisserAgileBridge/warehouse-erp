using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.CategoryTests;

public class CategoryUpdateDescriptionTests
{
    [Fact]
    public void UpdateDescription_UpdatesDescription()
    {
        var category = Category.Create("Beverages", "Old description");

        category.UpdateDescription("New description");

        Assert.Equal("New description", category.Description);
    }

    [Fact]
    public void UpdateDescription_ClearsDescriptionWhenNull()
    {
        var category = Category.Create("Beverages", "Old description");

        category.UpdateDescription(null);

        Assert.Null(category.Description);
    }

    [Fact]
    public void UpdateDescription_RejectsDescriptionLongerThanMaxLength()
    {
        var category = Category.Create("Beverages");
        var description = new string('a', Category.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => category.UpdateDescription(description));
    }
}
