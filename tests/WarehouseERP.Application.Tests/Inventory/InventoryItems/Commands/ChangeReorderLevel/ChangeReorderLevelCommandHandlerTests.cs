using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems.Commands.ChangeReorderLevel;
using WarehouseERP.Application.Tests.Common.Fakes;
using WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Commands.ChangeReorderLevel;

public class ChangeReorderLevelCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ChangesReorderLevel_WhenValid()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var unitOfWork = new FakeUnitOfWork();

        var inventoryItem = InventoryItem.Create(Guid.NewGuid(), Guid.NewGuid(), 10, reorderLevel: 2);
        inventoryItemRepository.Seed(inventoryItem);

        var handler = new ChangeReorderLevelCommandHandler(inventoryItemRepository, unitOfWork);

        var command = new ChangeReorderLevelCommand { InventoryItemId = inventoryItem.Id, ReorderLevel = 5 };

        var dto = await handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(5, dto.ReorderLevel);
        Assert.Equal(10, dto.QuantityOnHand);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ThrowsNotFoundException_WhenInventoryItemDoesNotExist()
    {
        var inventoryItemRepository = new FakeInventoryItemRepository();
        var unitOfWork = new FakeUnitOfWork();

        var handler = new ChangeReorderLevelCommandHandler(inventoryItemRepository, unitOfWork);

        var command = new ChangeReorderLevelCommand { InventoryItemId = Guid.NewGuid(), ReorderLevel = 5 };

        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command, CancellationToken.None));
    }
}
