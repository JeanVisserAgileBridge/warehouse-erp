using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItems;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Queries.GetInventoryItems;

public class GetInventoryItemsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllInventoryItems()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        inventoryItemRepository.Seed(InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid()));
        inventoryItemRepository.Seed(InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid()));

        var handler = new GetInventoryItemsQueryHandler(inventoryItemRepository);

        var result = await handler.HandleAsync(new GetInventoryItemsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
