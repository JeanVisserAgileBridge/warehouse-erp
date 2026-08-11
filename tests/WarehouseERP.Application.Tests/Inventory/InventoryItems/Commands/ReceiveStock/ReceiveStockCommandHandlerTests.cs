using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ReceiveStock;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Commands.ReceiveStock;

public class ReceiveStockCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_IncreasesQuantityOnHand_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new ReceiveStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceiveStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 5, Reference = "PO-001" };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(15, dto.QuantityOnHand);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenInventoryItemDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new ReceiveStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceiveStockCommand { InventoryItemId = Guid.NewGuid(), Quantity = 5 };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_CreatesReceiptStockMovement_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new ReceiveStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceiveStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 5, Reference = "PO-001" };

        await handler.HandleAsync(command, CancellationToken.None);

        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Receipt, movement.MovementType);
        Assert.Equal(5, movement.Quantity);
        Assert.Equal("PO-001", movement.Reference);
    }

    [Fact]
    public async Task HandleAsync_CommitsInventoryItemAndStockMovementInOneUnitOfWorkCall()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new ReceiveStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new ReceiveStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 5 };

        await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(1, inventoryItemRepository.UpdateCallCount);
        Assert.Equal(1, stockMovementRepository.AddCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }
}
