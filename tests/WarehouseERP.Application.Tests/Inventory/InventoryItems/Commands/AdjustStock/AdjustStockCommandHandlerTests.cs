using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.AdjustStock;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Commands.AdjustStock;

public class AdjustStockCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_SetsAbsoluteQuantityOnHand_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 25, Reference = "Cycle count" };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(25, dto.QuantityOnHand);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenInventoryItemDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = Guid.NewGuid(), NewQuantityOnHand = 25 };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_CreatesAdjustmentStockMovementWithAbsoluteDelta_WhenQuantityIncreases()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 25 };

        await handler.HandleAsync(command, CancellationToken.None);

        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Adjustment, movement.MovementType);
        Assert.Equal(15, movement.Quantity);
    }

    [Fact]
    public async Task HandleAsync_CreatesAdjustmentStockMovementWithAbsoluteDelta_WhenQuantityDecreases()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 2 };

        await handler.HandleAsync(command, CancellationToken.None);

        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Adjustment, movement.MovementType);
        Assert.Equal(8, movement.Quantity);
    }

    [Fact]
    public async Task HandleAsync_AllowsAdjustingQuantityOnHandToZero()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 0 };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(0, dto.QuantityOnHand);
        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem.Id, CancellationToken.None);
        Assert.Equal(10, Assert.Single(movements).Quantity);
    }

    [Fact]
    public async Task HandleAsync_CreatesNoStockMovement_WhenNewQuantityEqualsCurrentQuantity()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 10 };

        await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(0, stockMovementRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_CommitsInventoryItemAndStockMovementInOneUnitOfWorkCall()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new AdjustStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new AdjustStockCommand { InventoryItemId = inventoryItem.Id, NewQuantityOnHand = 25 };

        await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(1, inventoryItemRepository.UpdateCallCount);
        Assert.Equal(1, stockMovementRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }
}
