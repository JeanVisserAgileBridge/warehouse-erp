using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryByProductId;

public sealed class GetInventoryByProductIdQueryHandler : IQueryHandler<GetInventoryByProductIdQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public GetInventoryByProductIdQueryHandler(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<IReadOnlyList<InventoryItemDto>> HandleAsync(GetInventoryByProductIdQuery query, CancellationToken cancellationToken)
    {
        var inventoryItems = await _inventoryItemRepository.GetByProductIdAsync(query.ProductId, cancellationToken);

        return inventoryItems.Select(InventoryItemDto.FromDomain).ToList();
    }
}
