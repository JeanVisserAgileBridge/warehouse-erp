using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.AdjustStock;

public sealed class AdjustStockCommandHandler : ICommandHandler<AdjustStockCommand, InventoryItemDto>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdjustStockCommandHandler(
        IInventoryItemRepository inventoryItemRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InventoryItemDto> HandleAsync(AdjustStockCommand command, CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryItemRepository.GetByIdAsync(command.InventoryItemId, cancellationToken)
            ?? throw new NotFoundException($"Inventory item with id '{command.InventoryItemId}' was not found.");

        var quantityDelta = Math.Abs(command.NewQuantityOnHand - inventoryItem.QuantityOnHand);

        inventoryItem.AdjustStock(command.NewQuantityOnHand);

        await _inventoryItemRepository.UpdateAsync(inventoryItem, cancellationToken);

        // A zero-delta adjustment changed nothing, so no movement record is created (mirrors ChangeReorderLevel).
        if (quantityDelta > 0)
        {
            var stockMovement = StockMovement.Create(
                inventoryItem.Id, StockMovementType.Adjustment, quantityDelta, command.Reference);

            await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return InventoryItemDto.FromDomain(inventoryItem);
    }
}
