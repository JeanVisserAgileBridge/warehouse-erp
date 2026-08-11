using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.IssueStock;

public sealed class IssueStockCommandHandler : ICommandHandler<IssueStockCommand, InventoryItemDto>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IssueStockCommandHandler(
        IInventoryItemRepository inventoryItemRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InventoryItemDto> HandleAsync(IssueStockCommand command, CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryItemRepository.GetByIdAsync(command.InventoryItemId, cancellationToken)
            ?? throw new NotFoundException($"Inventory item with id '{command.InventoryItemId}' was not found.");

        inventoryItem.IssueStock(command.Quantity);

        var stockMovement = StockMovement.Create(
            inventoryItem.Id, StockMovementType.Issue, command.Quantity, command.Reference);

        await _inventoryItemRepository.UpdateAsync(inventoryItem, cancellationToken);
        await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return InventoryItemDto.FromDomain(inventoryItem);
    }
}
