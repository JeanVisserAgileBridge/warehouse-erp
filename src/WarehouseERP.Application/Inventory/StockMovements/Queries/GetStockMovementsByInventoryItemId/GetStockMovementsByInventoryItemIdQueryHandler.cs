using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;

public sealed class GetStockMovementsByInventoryItemIdQueryHandler
    : IQueryHandler<GetStockMovementsByInventoryItemIdQuery, IReadOnlyList<StockMovementDto>>
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public GetStockMovementsByInventoryItemIdQueryHandler(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<IReadOnlyList<StockMovementDto>> HandleAsync(GetStockMovementsByInventoryItemIdQuery query, CancellationToken cancellationToken)
    {
        var stockMovements = await _stockMovementRepository.GetByInventoryItemIdAsync(query.InventoryItemId, cancellationToken);

        return stockMovements.Select(StockMovementDto.FromDomain).ToList();
    }
}
