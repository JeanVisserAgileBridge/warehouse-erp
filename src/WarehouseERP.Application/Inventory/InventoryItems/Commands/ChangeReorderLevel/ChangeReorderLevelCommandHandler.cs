using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.ChangeReorderLevel;

public sealed class ChangeReorderLevelCommandHandler : ICommandHandler<ChangeReorderLevelCommand, InventoryItemDto>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeReorderLevelCommandHandler(IInventoryItemRepository inventoryItemRepository, IUnitOfWork unitOfWork)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InventoryItemDto> HandleAsync(ChangeReorderLevelCommand command, CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryItemRepository.GetByIdAsync(command.InventoryItemId, cancellationToken)
            ?? throw new NotFoundException($"Inventory item with id '{command.InventoryItemId}' was not found.");

        inventoryItem.ChangeReorderLevel(command.ReorderLevel);

        await _inventoryItemRepository.UpdateAsync(inventoryItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return InventoryItemDto.FromDomain(inventoryItem);
    }
}
