using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.StockMovementTests;

public class StockMovementCreateTests
{
    [Fact]
    public void Create_ReturnsStockMovementWithNonEmptyGuid()
    {
        var stockMovement = StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 10);

        Assert.NotEqual(Guid.Empty, stockMovement.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var inventoryItemId = Guid.NewGuid();

        var stockMovement = StockMovement.Create(inventoryItemId, StockMovementType.Receipt, 10, "PO-1001");

        Assert.Equal(inventoryItemId, stockMovement.InventoryItemId);
        Assert.Equal(StockMovementType.Receipt, stockMovement.MovementType);
        Assert.Equal(10, stockMovement.Quantity);
        Assert.Equal("PO-1001", stockMovement.Reference);
    }

    [Fact]
    public void Create_SetsOccurredAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var stockMovement = StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 10);

        var after = DateTime.UtcNow;
        Assert.InRange(stockMovement.OccurredAt, before, after);
    }

    [Fact]
    public void Create_RejectsEmptyInventoryItemId()
    {
        Assert.Throws<DomainException>(() => StockMovement.Create(Guid.Empty, StockMovementType.Receipt, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_RejectsZeroOrNegativeQuantity(int quantity)
    {
        Assert.Throws<DomainException>(() => StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, quantity));
    }

    [Fact]
    public void Create_AcceptsNullReference()
    {
        var stockMovement = StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 10);

        Assert.Null(stockMovement.Reference);
    }

    [Fact]
    public void Create_AcceptsReferenceAtMaxLength()
    {
        var reference = new string('a', StockMovement.MaxReferenceLength);

        var stockMovement = StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 10, reference);

        Assert.Equal(reference, stockMovement.Reference);
    }

    [Fact]
    public void Create_RejectsReferenceLongerThanMaxLength()
    {
        var reference = new string('a', StockMovement.MaxReferenceLength + 1);

        Assert.Throws<DomainException>(() => StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 10, reference));
    }
}
