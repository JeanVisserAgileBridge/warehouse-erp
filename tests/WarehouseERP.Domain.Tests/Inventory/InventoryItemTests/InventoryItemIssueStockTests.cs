using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.InventoryItemTests;

public class InventoryItemIssueStockTests
{
    [Fact]
    public void IssueStock_DecreasesQuantityOnHand()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        inventoryItem.IssueStock(4);

        Assert.Equal(6, inventoryItem.QuantityOnHand);
    }

    [Fact]
    public void IssueStock_UpdatesUpdatedAt()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        inventoryItem.IssueStock(4);

        Assert.True(inventoryItem.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void IssueStock_AllowsReducingQuantityOnHandToZero()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        inventoryItem.IssueStock(10);

        Assert.Equal(0, inventoryItem.QuantityOnHand);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IssueStock_RejectsZeroOrNegativeQuantity(int quantity)
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        Assert.Throws<DomainException>(() => inventoryItem.IssueStock(quantity));
    }

    [Fact]
    public void IssueStock_RejectsQuantityGreaterThanQuantityOnHand()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);

        Assert.Throws<DomainException>(() => inventoryItem.IssueStock(11));
    }

    [Fact]
    public void IssueStock_DoesNotChangeStateWhenQuantityExceedsQuantityOnHand()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalQuantityOnHand = inventoryItem.QuantityOnHand;
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        Assert.Throws<DomainException>(() => inventoryItem.IssueStock(11));

        Assert.Equal(originalQuantityOnHand, inventoryItem.QuantityOnHand);
        Assert.Equal(originalUpdatedAt, inventoryItem.UpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IssueStock_DoesNotChangeStateWhenRejected(int quantity)
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        var originalQuantityOnHand = inventoryItem.QuantityOnHand;
        var originalUpdatedAt = inventoryItem.UpdatedAt;

        Assert.Throws<DomainException>(() => inventoryItem.IssueStock(quantity));

        Assert.Equal(originalQuantityOnHand, inventoryItem.QuantityOnHand);
        Assert.Equal(originalUpdatedAt, inventoryItem.UpdatedAt);
    }
}
