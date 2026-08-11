using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItemById;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Queries.GetInventoryItemById;

public class GetInventoryItemByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsMatchingInventoryItem_WhenFound()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new GetInventoryItemByIdQueryHandler(inventoryItemRepository);

        var dto = await handler.HandleAsync(new GetInventoryItemByIdQuery { Id = inventoryItem.Id }, CancellationToken.None);

        Assert.Equal(inventoryItem.Id, dto.Id);
        Assert.Equal(10, dto.QuantityOnHand);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenNotFound()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var handler = new GetInventoryItemByIdQueryHandler(inventoryItemRepository);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.HandleAsync(new GetInventoryItemByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None));
    }
}
