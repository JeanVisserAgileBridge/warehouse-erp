using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.InventoryItemTests;

public class InventoryItemChangeReorderLevelTests
{
    [Fact]
    public void ChangeReorderLevel_UpdatesReorderLevel()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 2);

        inventoryItem.ChangeReorderLevel(5);

        Assert.Equal(5, inventoryItem.ReorderLevel);
    }

    [Fact]
    public void ChangeReorderLevel_UpdatesUpdatedAt()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 2);
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        inventoryItem.ChangeReorderLevel(5);

        Assert.True(inventoryItem.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeReorderLevel_AcceptsZero()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 2);

        inventoryItem.ChangeReorderLevel(0);

        Assert.Equal(0, inventoryItem.ReorderLevel);
    }

    [Fact]
    public void ChangeReorderLevel_RejectsNegativeValue()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 2);

        Assert.Throws<DomainException>(() => inventoryItem.ChangeReorderLevel(-1));
    }

    [Fact]
    public void ChangeReorderLevel_DoesNotChangeStateWhenRejected()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 2);
        var originalReorderLevel = inventoryItem.ReorderLevel;
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        Assert.Throws<DomainException>(() => inventoryItem.ChangeReorderLevel(-1));

        Assert.Equal(originalReorderLevel, inventoryItem.ReorderLevel);
        Assert.Equal(originalUpdatedAt, inventoryItem.UpdatedAt);
    }
}
