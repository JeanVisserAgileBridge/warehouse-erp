using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.WarehouseTests;

public class WarehouseChangeAddressTests
{
    [Fact]
    public void ChangeAddress_UpdatesAddress()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse", "1 Industrial Way");

        warehouse.ChangeAddress("2 Commerce Street");

        Assert.Equal("2 Commerce Street", warehouse.Address);
    }

    [Fact]
    public void ChangeAddress_ClearsAddressWhenNull()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse", "1 Industrial Way");

        warehouse.ChangeAddress(null);

        Assert.Null(warehouse.Address);
    }

    [Fact]
    public void ChangeAddress_UpdatesUpdatedAt()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var originalUpdatedAt = warehouse.UpdatedAt;

        warehouse.ChangeAddress("2 Commerce Street");

        Assert.True(warehouse.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeAddress_RejectsAddressLongerThanMaxLength()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var address = new string('a', Warehouse.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => warehouse.ChangeAddress(address));
    }

    [Fact]
    public void ChangeAddress_AcceptsAddressAtMaxLength()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var address = new string('a', Warehouse.MaxAddressLength);

        warehouse.ChangeAddress(address);

        Assert.Equal(address, warehouse.Address);
    }
}
