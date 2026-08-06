using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.WarehouseTests;

public class WarehouseRenameTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        warehouse.Rename("Central Warehouse");

        Assert.Equal("Central Warehouse", warehouse.Name);
    }

    [Fact]
    public void Rename_UpdatesUpdatedAt()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var originalUpdatedAt = warehouse.UpdatedAt;

        warehouse.Rename("Central Warehouse");

        Assert.True(warehouse.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.Throws<DomainException>(() => warehouse.Rename(name!));
    }

    [Fact]
    public void Rename_RejectsNameLongerThanMaxLength()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var name = new string('a', Warehouse.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => warehouse.Rename(name));
    }
}
