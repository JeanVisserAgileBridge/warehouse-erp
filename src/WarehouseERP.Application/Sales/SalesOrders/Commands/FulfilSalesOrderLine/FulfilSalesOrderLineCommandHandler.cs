using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.FulfilSalesOrderLine;

// Fulfilment spans three aggregates: SalesOrder, InventoryItem, and StockMovement. All three
// are written through repositories that do not self-commit, and this handler commits them
// together with a single IUnitOfWork.SaveChangesAsync call so the fulfilment is atomic.
//
// Unlike Purchase Order receiving, the InventoryItem must already exist: stock cannot be issued
// from a product/location combination that has never been stocked.
public sealed class FulfilSalesOrderLineCommandHandler : ICommandHandler<FulfilSalesOrderLineCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FulfilSalesOrderLineCommandHandler(
        ISalesOrderRepository salesOrderRepository,
        IStorageLocationRepository storageLocationRepository,
        IInventoryItemRepository inventoryItemRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _storageLocationRepository = storageLocationRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(FulfilSalesOrderLineCommand command, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{command.SalesOrderId}' was not found.");

        // Enforces: order must exist, must be in a fulfillable status, the line must be on the
        // order, quantity must be greater than zero, and it must not exceed quantity ordered.
        salesOrder.FulfillProduct(command.ProductId, command.Quantity);

        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.StorageLocationId, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.StorageLocationId}' was not found.");

        if (!storageLocation.IsActive)
        {
            throw new InactiveStorageLocationException($"Storage location with id '{command.StorageLocationId}' is not active.");
        }

        var inventoryItem = await _inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(
            command.ProductId, command.StorageLocationId, cancellationToken)
            ?? throw new NotFoundException(
                $"No inventory item exists for product '{command.ProductId}' at storage location '{command.StorageLocationId}'.");

        inventoryItem.IssueStock(command.Quantity);
        await _inventoryItemRepository.UpdateAsync(inventoryItem, cancellationToken);

        var stockMovement = StockMovement.Create(
            inventoryItem.Id, StockMovementType.Issue, command.Quantity, command.Reference);
        await _stockMovementRepository.AddAsync(stockMovement, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
