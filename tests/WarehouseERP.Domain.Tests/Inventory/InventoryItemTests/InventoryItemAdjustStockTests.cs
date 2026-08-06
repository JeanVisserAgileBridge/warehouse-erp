using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.InventoryItemTests;

public class InventoryItemAdjustStockTests
{
    [Fact]
    public void AdjustStock_SetsAbsoluteQuantityOnHand()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        inventoryItem.AdjustStock(50);

        Assert.Equal(50, inventoryItem.QuantityOnHand);
    }

    [Fact]
    public void AdjustStock_AllowsSettingQuantityOnHandToZero()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        inventoryItem.AdjustStock(0);

        Assert.Equal(0, inventoryItem.QuantityOnHand);
    }

    [Fact]
    public void AdjustStock_UpdatesUpdatedAt()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        inventoryItem.AdjustStock(50);

        Assert.True(inventoryItem.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void AdjustStock_RejectsNegativeValue()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        Assert.Throws<DomainException>(() => inventoryItem.AdjustStock(-1));
    }

    [Fact]
    public void AdjustStock_DoesNotChangeStateWhenRejected()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalQuantityOnHand = inventoryItem.QuantityOnHand;
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        Assert.Throws<DomainException>(() => inventoryItem.AdjustStock(-1));

        Assert.Equal(originalQuantityOnHand, inventoryItem.QuantityOnHand);
        Assert.Equal(originalUpdatedAt, inventoryItem.UpdatedAt);
    }
}
