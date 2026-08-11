using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByStorageLocationId;

public sealed class GetInventoryByStorageLocationIdQueryHandler : IQueryHandler<GetInventoryByStorageLocationIdQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public GetInventoryByStorageLocationIdQueryHandler(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<IReadOnlyList<InventoryItemDto>> HandleAsync(GetInventoryByStorageLocationIdQuery query, CancellationToken cancellationToken)
    {
        var inventoryItems = await _inventoryItemRepository.GetByStorageLocationIdAsync(query.StorageLocationId, cancellationToken);

        return inventoryItems.Select(InventoryItemDto.FromDomain).ToList();
    }
}
