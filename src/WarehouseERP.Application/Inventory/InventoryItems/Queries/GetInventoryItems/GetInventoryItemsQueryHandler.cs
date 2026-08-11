using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItems;

public sealed class GetInventoryItemsQueryHandler : IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public GetInventoryItemsQueryHandler(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<IReadOnlyList<InventoryItemDto>> HandleAsync(GetInventoryItemsQuery query, CancellationToken cancellationToken)
    {
        var inventoryItems = await _inventoryItemRepository.GetAllAsync(cancellationToken);

        return inventoryItems.Select(InventoryItemDto.FromDomain).ToList();
    }
}
