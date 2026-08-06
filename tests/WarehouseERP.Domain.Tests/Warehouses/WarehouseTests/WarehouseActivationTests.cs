using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.WarehouseTests;

public class WarehouseActivationTests
{
    [Fact]
    public void Deactivate_MakesWarehouseInactive()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        warehouse.Deactivate();

        Assert.False(warehouse.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        warehouse.Deactivate();
        warehouse.Deactivate();

        Assert.False(warehouse.IsActive);
    }

    [Fact]
    public void Deactivate_UpdatesUpdatedAt()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        var originalUpdatedAt = warehouse.UpdatedAt;

        warehouse.Deactivate();

        Assert.True(warehouse.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Activate_MakesInactiveWarehouseActive()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouse.Deactivate();

        warehouse.Activate();

        Assert.True(warehouse.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");

        warehouse.Activate();
        warehouse.Activate();

        Assert.True(warehouse.IsActive);
    }

    [Fact]
    public void Activate_UpdatesUpdatedAt()
    {
        var warehouse = Warehouse.Create("WH-01", "Main Warehouse");
        warehouse.Deactivate();
        var originalUpdatedAt = warehouse.UpdatedAt;

        warehouse.Activate();

        Assert.True(warehouse.UpdatedAt >= originalUpdatedAt);
    }
}
