using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Inventory.InventoryItems.Queries.GetInventoryItemById;

public sealed class GetInventoryItemByIdQueryHandler : IQueryHandler<GetInventoryItemByIdQuery, InventoryItemDto>
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public GetInventoryItemByIdQueryHandler(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<InventoryItemDto> HandleAsync(GetInventoryItemByIdQuery query, CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryItemRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Inventory item with id '{query.Id}' was not found.");

        return InventoryItemDto.FromDomain(inventoryItem);
    }
}
