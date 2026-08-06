using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.InventoryItemTests;

public class InventoryItemReceiveStockTests
{
    [Fact]
    public void ReceiveStock_IncreasesQuantityOnHand()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        inventoryItem.ReceiveStock(5);

        Assert.Equal(15, inventoryItem.QuantityOnHand);
    }

    [Fact]
    public void ReceiveStock_UpdatesUpdatedAt()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        inventoryItem.ReceiveStock(5);

        Assert.True(inventoryItem.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReceiveStock_RejectsZeroOrNegativeQuantity(int quantity)
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        Assert.Throws<DomainException>(() => inventoryItem.ReceiveStock(quantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReceiveStock_DoesNotChangeStateWhenRejected(int quantity)
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalQuantityOnHand = inventoryItem.QuantityOnHand;
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        Assert.Throws<DomainException>(() => inventoryItem.ReceiveStock(quantity));

        Assert.Equal(originalQuantityOnHand, inventoryItem.QuantityOnHand);
        Assert.Equal(originalUpdatedAt, inventoryItem.UpdatedAt);
    }
}
