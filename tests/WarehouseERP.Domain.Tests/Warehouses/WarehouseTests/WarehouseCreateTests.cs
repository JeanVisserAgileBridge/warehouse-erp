using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.WarehouseTests;

public class WarehouseCreateTests
{
    [Fact]
    public void Create_ReturnsWarehouseWithNonEmptyGuid()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.NotEqual(Guid.Empty, warehouse.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse", "1 Industrial Way");

        Assert.Equal("WH-01", warehouse.Code);
        Assert.Equal("Main Warehouse", warehouse.Name);
        Assert.Equal("1 Industrial Way", warehouse.Address);
    }

    [Fact]
    public void Create_MakesWarehouseActive()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.True(warehouse.IsActive);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.Equal(warehouse.CreatedAt, warehouse.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullAddress()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.Null(warehouse.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceCode(string? code)
    {
        Assert.Throws<DomainException>(() => Warehouse.Create(code!, "Main Warehouse"));
    }

    [Fact]
    public void Create_RejectsCodeLongerThanMaxLength()
    {
        var code = new string('a', Warehouse.MaxCodeLength + 1);

        Assert.Throws<DomainException>(() => Warehouse.Create(code, "Main Warehouse"));
    }

    [Fact]
    public void Create_AcceptsCodeAtMaxLength()
    {
        var code = new string('a', Warehouse.MaxCodeLength);

        var warehouse = Warehouse.Create(code, "Main Warehouse");

        Assert.Equal(code, warehouse.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        Assert.Throws<DomainException>(() => Warehouse.Create("WH-01", name!));
    }

    [Fact]
    public void Create_RejectsNameLongerThanMaxLength()
    {
        var name = new string('a', Warehouse.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => Warehouse.Create("WH-01", name));
    }

    [Fact]
    public void Create_AcceptsNameAtMaxLength()
    {
        var name = new string('a', Warehouse.MaxNameLength);

        var warehouse = Warehouse.Create("WH-01", name);

        Assert.Equal(name, warehouse.Name);
    }

    [Fact]
    public void Create_RejectsAddressLongerThanMaxLength()
    {
        var address = new string('a', Warehouse.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => Warehouse.Create("WH-01", "Main Warehouse", address));
    }

    [Fact]
    public void Create_AcceptsAddressAtMaxLength()
    {
        var address = new string('a', Warehouse.MaxAddressLength);

        var warehouse = Warehouse.Create("WH-01", "Main Warehouse", address);

        Assert.Equal(address, warehouse.Address);
    }
}
