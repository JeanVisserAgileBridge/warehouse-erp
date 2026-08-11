using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.StockMovements.Fakes;

public sealed class FakeStockMovementRepository : IStockMovementRepository
{
    private readonly List<StockMovement> _stockMovements = new();

    public CancellationToken? LastCancellationToken { get; private set; }
    public int AddCallCount { get; private set; }

    public void Seed(StockMovement stockMovement)
    {
        _stockMovements.Add(stockMovement);
    }

    public Task<IReadOnlyList<StockMovement>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<StockMovement>>(
            _stockMovements.Where(stockMovement => stockMovement.InventoryItemId == inventoryItemId).ToList());
    }

    public Task AddAsync(StockMovement stockMovement, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        AddCallCount++;
        _stockMovements.Add(stockMovement);
        return Task.CompletedTask;
    }
}
