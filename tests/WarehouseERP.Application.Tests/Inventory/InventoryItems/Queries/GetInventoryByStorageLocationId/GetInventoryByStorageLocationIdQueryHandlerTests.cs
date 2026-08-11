using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByStorageLocationId;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Queries.GetInventoryByStorageLocationId;

public class GetInventoryByStorageLocationIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlyInventoryItemsForStorageLocation()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var storageLocationId = Guid.NewGuid();
        inventoryItemRepository.Seed(InventoryItem.Create(Guid.NewGuid(), storageLocationId));
        inventoryItemRepository.Seed(InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid()));

        var handler = new GetInventoryByStorageLocationIdQueryHandler(inventoryItemRepository);

        var result = await handler.HandleAsync(
            new GetInventoryByStorageLocationIdQuery { StorageLocationId = storageLocationId }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(storageLocationId, result[0].StorageLocationId);
    }
}
