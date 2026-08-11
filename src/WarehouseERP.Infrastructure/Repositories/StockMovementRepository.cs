using Microsoft.EntityFrameworkCore;
using WarehouseERP.Application.Inventory.StockMovements;
using WarehouseERP.Domain.Inventory;
using WarehouseERP.Infrastructure.Persistence;

namespace WarehouseERP.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly WarehouseErpDbContext _context;

    public StockMovementRepository(WarehouseErpDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<StockMovement>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken)
    {
        return await _context.StockMovements
            .AsNoTracking()
            .Where(stockMovement => stockMovement.InventoryItemId == inventoryItemId)
            .OrderByDescending(stockMovement => stockMovement.OccurredAt)
            .ToListAsync(cancellationToken);
    }

    // Does not call SaveChangesAsync: this must commit together with the related
    // InventoryItem change, so the caller's IUnitOfWork owns the commit point.
    public async Task AddAsync(StockMovement stockMovement, CancellationToken cancellationToken)
    {
        await _context.StockMovements.AddAsync(stockMovement, cancellationToken);
    }
}
