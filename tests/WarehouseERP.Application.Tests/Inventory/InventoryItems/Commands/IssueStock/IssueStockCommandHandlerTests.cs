using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.IssueStock;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;
using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Commands.IssueStock;

public class IssueStockCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DecreasesQuantityOnHand_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new IssueStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new IssueStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 4, Reference = "SO-001" };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(6, dto.QuantityOnHand);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenInventoryItemDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new IssueStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new IssueStockCommand { InventoryItemId = Guid.NewGuid(), Quantity = 4 };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_ThrowsDomainException_WhenQuantityExceedsQuantityOnHand()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 3);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new IssueStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new IssueStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 4 };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));
    }

    [Fact]
    public async Task HandleAsync_DoesNotCommitAnything_WhenIssueIsRejectedByDomain()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 3);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new IssueStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new IssueStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 4 };

        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal(0, inventoryItemRepository.UpdateCallCount);
        Assert.Equal(0, stockMovementRepository.AddCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_CreatesIssueStockMovement_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var stockMovementRepository = new FakeStockMovementRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new IssueStockCommandHandler(inventoryItemRepository, stockMovementRepository, unitOfWork);

        var command = new IssueStockCommand { InventoryItemId = inventoryItem.Id, Quantity = 4, Reference = "SO-001" };

        await handler.HandleAsync(command, CancellationToken.None);

        var movements = await stockMovementRepository.GetByInventoryItemIdAsync(inventoryItem.Id, CancellationToken.None);
        var movement = Assert.Single(movements);
        Assert.Equal(StockMovementType.Issue, movement.MovementType);
        Assert.Equal(4, movement.Quantity);
        Assert.Equal("SO-001", movement.Reference);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }
}
