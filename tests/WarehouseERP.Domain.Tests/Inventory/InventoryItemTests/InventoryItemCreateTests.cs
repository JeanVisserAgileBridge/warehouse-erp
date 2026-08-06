using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Domain.Tests.Inventory.InventoryItemTests;

public class InventoryItemCreateTests
{
    [Fact]
    public void Create_ReturnsInventoryItemWithNonEmptyGuid()
    {
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid());

        Assert.NotEqual(Guid.Empty, inventoryItem.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var productId = Guid.NewGuid();
        var storageLocationId = Guid.NewGuid();

        var inventoryItem = InventoryItem.Create(productId, storageLocationId, 10, 2);

        Assert.Equal(productId, inventoryItem.ProductId);
        Assert.Equal(storageLocationId, inventoryItem.StorageLocationId);
        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(2, inventoryItem.ReorderLevel);
    }

    [Fact]
    public void Create_RejectsEmptyProductId()
    {
        Assert.Throws<DomainException>(() => InventoryItem.Create(Guid.Empty, Guid.NewGuid()));
    }

    [Fact]
    public void Create_RejectsEmptyStorageLocationId()
    {
        Assert.Throws<DomainException>(() => InventoryItem.Create(Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void Create_RejectsNegativeQuantityOnHand()
    {
        Assert.Throws<DomainException>(() => InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), -1));
    }

    [Fact]
    public void Create_RejectsNegativeReorderLevel()
    {
        Assert.Throws<DomainException>(() => InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 0, -1));
    }
}
