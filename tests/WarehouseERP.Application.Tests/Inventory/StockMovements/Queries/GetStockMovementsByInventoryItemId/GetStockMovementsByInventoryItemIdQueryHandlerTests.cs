using WarehouseERP.Application.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;

public class GetStockMovementsByInventoryItemIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsOnlyMovementsForInventoryItem()
    {
        var stockMovementRepository = new FakeStockMovementRepository();
        var inventoryItemId = Guid.NewGuid();
        stockMovementRepository.Seed(StockMovement.Create(inventoryItemId, StockMovementType.Receipt, 10));
        stockMovementRepository.Seed(StockMovement.Create(inventoryItemId, StockMovementType.Issue, 4));
        stockMovementRepository.Seed(StockMovement.Create(Guid.NewGuid(), StockMovementType.Receipt, 20));

        var handler = new GetStockMovementsByInventoryItemIdQueryHandler(stockMovementRepository);

        var result = await handler.HandleAsync(
            new GetStockMovementsByInventoryItemIdQuery { InventoryItemId = inventoryItemId }, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, dto => Assert.Equal(inventoryItemId, dto.InventoryItemId));
    }
}
