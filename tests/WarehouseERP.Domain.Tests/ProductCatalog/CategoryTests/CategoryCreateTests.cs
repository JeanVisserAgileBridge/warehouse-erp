using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.CategoryTests;

public class CategoryCreateTests
{
    [Fact]
    public void Create_ReturnsCategoryWithNonEmptyGuid()
    {
        var category = Category.Create("Beverages");

        Assert.NotEqual(Guid.Empty, category.Id);
    }

    [Fact]
    public void Create_StoresSuppliedNameAndDescription()
    {
        var category = Category.Create("Beverages", "Drinks and refreshments");

        Assert.Equal("Beverages", category.Name);
        Assert.Equal("Drinks and refreshments", category.Description);
    }

    [Fact]
    public void Create_MakesCategoryActive()
    {
        var category = Category.Create("Beverages");

        Assert.True(category.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        Assert.Throws<DomainException>(() => Category.Create(name!));
    }

    [Fact]
    public void Create_RejectsNameLongerThanMaxLength()
    {
        var name = new string('a', Category.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => Category.Create(name));
    }

    [Fact]
    public void Create_AcceptsNameAtMaxLength()
    {
        var name = new string('a', Category.MaxNameLength);

        var category = Category.Create(name);

        Assert.Equal(name, category.Name);
    }

    [Fact]
    public void Create_AcceptsNullDescription()
    {
        var category = Category.Create("Beverages", null);

        Assert.Null(category.Description);
    }

    [Fact]
    public void Create_RejectsDescriptionLongerThanMaxLength()
    {
        var description = new string('a', Category.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => Category.Create("Beverages", description));
    }

    [Fact]
    public void Create_AcceptsDescriptionAtMaxLength()
    {
        var description = new string('a', Category.MaxDescriptionLength);

        var category = Category.Create("Beverages", description);

        Assert.Equal(description, category.Description);
    }
}
