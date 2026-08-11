using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;

// Receiving spans three aggregates: PurchaseOrder, InventoryItem, and StockMovement. All three
// are written through repositories that do not self-commit, and this handler commits them
// together with a single IUnitOfWork.SaveChangesAsync call so the receipt is atomic.
public sealed class ReceivePurchaseOrderLineCommandHandler : ICommandHandler<ReceivePurchaseOrderLineCommand, PurchaseOrderDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReceivePurchaseOrderLineCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IStorageLocationRepository storageLocationRepository,
        IInventoryItemRepository inventoryItemRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _storageLocationRepository = storageLocationRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> HandleAsync(ReceivePurchaseOrderLineCommand command, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException($"Purchase order with id '{command.PurchaseOrderId}' was not found.");

        // Enforces: order must exist, must be in a receivable status, the line must be on the
        // order, quantity must be greater than zero, and it must not exceed quantity ordered.
        purchaseOrder.ReceiveProduct(command.ProductId, command.Quantity);

        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.StorageLocationId, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.StorageLocationId}' was not found.");

        if (!storageLocation.IsActive)
        {
            throw new InactiveStorageLocationException($"Storage location with id '{command.StorageLocationId}' is not active.");
        }

        var inventoryItem = await _inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(
            command.ProductId, command.StorageLocationId, cancellationToken);

        if (inventoryItem is not null)
        {
            inventoryItem.ReceiveStock(command.Quantity);
            await _inventoryItemRepository.UpdateAsync(inventoryItem, cancellationToken);
        }
        else
        {
            inventoryItem = InventoryItem.Create(command.ProductId, command.StorageLocationId, command.Quantity);
            await _inventoryItemRepository.AddAsync(inventoryItem, cancellationToken);
        }

        var stockMovement = StockMovement.Create(
            inventoryItem.Id, StockMovementType.Receipt, command.Quantity, command.Reference);
        await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PurchaseOrderDto.FromDomain(purchaseOrder);
    }
}
