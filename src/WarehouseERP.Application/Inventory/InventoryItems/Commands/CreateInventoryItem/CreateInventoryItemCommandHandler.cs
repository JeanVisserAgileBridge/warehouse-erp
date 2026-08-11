using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Application.ProductCatalog.Products;
using WarehouseERP.Application.Warehouses.StorageLocations;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.CreateInventoryItem;

public sealed class CreateInventoryItemCommandHandler : ICommandHandler<CreateInventoryItemCommand, InventoryItemDto>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStorageLocationRepository _storageLocationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInventoryItemCommandHandler(
        IInventoryItemRepository inventoryItemRepository,
        IProductRepository productRepository,
        IStorageLocationRepository storageLocationRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryItemRepository = inventoryItemRepository;
        _productRepository = productRepository;
        _storageLocationRepository = storageLocationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InventoryItemDto> HandleAsync(CreateInventoryItemCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new NotFoundException($"Product with id '{command.ProductId}' was not found.");

        if (!product.IsActive)
        {
            throw new InactiveProductException($"Product with id '{command.ProductId}' is not active.");
        }

        var storageLocation = await _storageLocationRepository.GetByIdAsync(command.StorageLocationId, cancellationToken)
            ?? throw new NotFoundException($"Storage location with id '{command.StorageLocationId}' was not found.");

        if (!storageLocation.IsActive)
        {
            throw new InactiveStorageLocationException($"Storage location with id '{command.StorageLocationId}' is not active.");
        }

        var existingInventoryItem = await _inventoryItemRepository.GetByProductIdAndStorageLocationIdAsync(
            command.ProductId, command.StorageLocationId, cancellationToken);
        if (existingInventoryItem is not null)
        {
            throw new DuplicateInventoryItemException(
                $"An inventory item for product '{command.ProductId}' at storage location '{command.StorageLocationId}' already exists.");
        }

        var inventoryItem = InventoryItem.Create(
            command.ProductId, command.StorageLocationId, command.QuantityOnHand, command.ReorderLevel);

        await _inventoryItemRepository.AddAsync(inventoryItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return InventoryItemDto.FromDomain(inventoryItem);
    }
}
