using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.WarehouseTests;

public class WarehouseChangeCodeTests
{
    [Fact]
    public void ChangeCode_UpdatesCode()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        warehouse.ChangeCode("WH-02");

        Assert.Equal("WH-02", warehouse.Code);
    }

    [Fact]
    public void ChangeCode_UpdatesUpdatedAt()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var originalUpdatedAt = warehouse.UpdatedAt;

        warehouse.ChangeCode("WH-02");

        Assert.True(warehouse.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeCode_RejectsNullEmptyOrWhitespaceCode(string? code)
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        Assert.Throws<DomainException>(() => warehouse.ChangeCode(code!));
    }

    [Fact]
    public void ChangeCode_RejectsCodeLongerThanMaxLength()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var code = new string('a', Warehouse.MaxCodeLength + 1);

        Assert.Throws<DomainException>(() => warehouse.ChangeCode(code));
    }

    [Fact]
    public void ChangeCode_AcceptsCodeAtMaxLength()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var code = new string('a', Warehouse.MaxCodeLength);

        warehouse.ChangeCode(code);

        Assert.Equal(code, warehouse.Code);
    }
}
