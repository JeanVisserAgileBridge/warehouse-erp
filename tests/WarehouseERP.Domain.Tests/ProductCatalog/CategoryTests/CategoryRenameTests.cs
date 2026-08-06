using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.ProductCatalog;

namespace WarehouseERP.Domain.Tests.ProductCatalog.CategoryTests;

public class CategoryRenameTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var category = Category.Create("Beverages");

        category.Rename("Snacks");

        Assert.Equal("Snacks", category.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        var category = Category.Create("Beverages");

        Assert.Throws<DomainException>(() => category.Rename(name!));
    }

    [Fact]
    public void Rename_RejectsNameLongerThanMaxLength()
    {
        var category = Category.Create("Beverages");
        var name = new string('a', Category.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => category.Rename(name));
    }
}
