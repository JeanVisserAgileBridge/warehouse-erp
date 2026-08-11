using WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByProductId;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlyInventoryItemsForProduct()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var productId = Guid.NewGuid();
        inventoryItemRepository.Seed(InventoryItem.Create(productId, Guid.NewGuid()));
        inventoryItemRepository.Seed(InventoryItem.Create(productId, Guid.NewGuid()));
        inventoryItemRepository.Seed(InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid()));

        var handler = new GetInventoryByProductIdQueryHandler(inventoryItemRepository);

        var result = await handler.HandleAsync(new GetInventoryByProductIdQuery { ProductId = productId }, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, dto => Assert.Equal(productId, dto.ProductId));
    }
}
